using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    // Patrolling: Enemy walks through set of waypoints until it sees the player
    // Alerted: Enemy briefly detected the player, walk to the last known position to investigate
    // Hunting: Enemy has seen the player again while in the Alerted state, start chasing and shooting
    public enum EnemyState { Patrolling, Alerted, Hunting }

    [SerializeField] private Vector3 visionOffset;

    [SerializeField] private UnityEvent<EnemyState> onStateChanged;

    [Header("Patrolling")]
    [Tooltip("Probability of the enemy stopping for a few seconds upon reaching a waypoint")]
    [SerializeField] [Range(0.0f, 1.0f)] private float pauseChance = 0.3f;
    [SerializeField] private List<Transform> waypoints;
    private int waypointIndex = 0;

    [Header("Alerted")]
    [SerializeField] private float alertMaxDistance = 30.0f;
    [SerializeField] private float alertFieldOfView = 180.0f;
    [Tooltip("After how many seconds of being in the alerted state idle at the player's last known position this enemy will return to patrolling")]
    [SerializeField] private float cancelAlertTime = 5.0f;
    private float alertTime = 0.0f; // Timestamp for when alerted state was entered

    [Header("Hunting")]
    [Tooltip("For how many seconds enemy will remain in the hunt state after losing sight of the player")]
    [SerializeField] private float huntTimeout = 5.0f;
    [SerializeField] private float shootingRange = 50.0f;
    [Tooltip("How many seconds between each shot")]
    [SerializeField] private float shootingDelay = 0.5f;

	private EnemyState state = EnemyState.Patrolling;
	private NavMeshAgent navMeshAgent;
    private CharacterMovement characterMovement;
    private bool pausePatrolling = false;
    private Transform playerTransform;
    private bool shootCooldown = false;

	public EnemyState State { get => state; set { state = value; onStateChanged.Invoke(value); } }
    public int WaypointIndex { get => waypointIndex; }
    public float AlertTime { get => alertTime; }
    public Vector3 Destination { get => navMeshAgent.destination; }
    public UnityEvent<EnemyState> OnStateChanged { get => onStateChanged; }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterMovement = GetComponent<CharacterMovement>();

        navMeshAgent.speed = characterMovement.WalkSpeed;
        navMeshAgent.angularSpeed = characterMovement.RotationSpeed;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Set waypoint index to the closest waypoint
        if (waypoints.Count > 1)
        {
            SelectClosestWaypoint();
        }
        else if (waypoints.Count == 1)
        {
            navMeshAgent.destination = waypoints[0].position;
        }
    }

    private void Update()
    {
        // Update state machine
        switch (state)
        {
            case EnemyState.Patrolling:
                UpdatePatrolling();
                break;
            case EnemyState.Alerted:
                UpdateAlerted();
                break;
            case EnemyState.Hunting:
                UpdateHunting();
                break;
        }

        characterMovement.Animator.SetFloat("Vertical", navMeshAgent.velocity.z);
    }

    public void Load(EnemyState state, int waypointIndex, float alertTime, Vector3 destination, float health)
    {
        State = state;
        this.waypointIndex = waypointIndex;
        this.alertTime = alertTime;
        navMeshAgent.destination = destination;
        GetComponent<Health>().Load(health);
    }

    public void AlertToPosition(Vector3 position)
    {
        if (state == EnemyState.Hunting)
            return;

        State = EnemyState.Alerted;
        alertTime = Time.time;
        navMeshAgent.destination = position;
    }

    private void UpdatePatrolling()
    {
		// If reached waypoint, move to next one
		if (!pausePatrolling && Vector3.Distance(navMeshAgent.destination, transform.position) < 0.5f)
        {
            // Have a random chance of stopping for a short while and then 
            if (Random.value <= pauseChance)
            {
                // Stop for a short amount of time
                pausePatrolling = true;
                Invoke(nameof(SelectNewWaypoint), Random.Range(2.0f, 5.0f));
            }
            else
            {
                // Continue immediately to the next waypoint
                SelectNewWaypoint();
            }
		}

        // Check if player can be seen
        if (CanSeePlayer())
        {
			State = EnemyState.Alerted;
			alertTime = Time.time;
            navMeshAgent.destination = playerTransform.position;
		}
	}

    private void UpdateAlerted()
    {
		if (CanSeePlayer())
		{
			navMeshAgent.destination = playerTransform.position;

            // Enter hunting mode if the player is seen again at least 2 seconds after first contact
            if (Time.time - alertTime >= 2.0f)
            {
                State = EnemyState.Hunting;
            }
		}
        else
        {
            // Check if alert state should be cancelled
            if (Time.time - alertTime >= cancelAlertTime && Vector3.Distance(transform.position, navMeshAgent.destination) < 1.0f)
            {
                State = EnemyState.Patrolling;
                SelectClosestWaypoint();
            }
        }
	}

    private void UpdateHunting()
    {
        // Go back to alert state if player is no longer visible
        if (!CanSeePlayer())
        {
			State = EnemyState.Alerted;
			alertTime = Time.time;
			navMeshAgent.destination = playerTransform.position;
			navMeshAgent.isStopped = false;
			characterMovement.Animator.SetBool("Aiming", false);
			return;
		}

		// Face the player
		transform.rotation = Quaternion.Euler(0.0f, Quaternion.LookRotation((playerTransform.position - transform.position).normalized, Vector3.up).eulerAngles.y, 0.0f);

		float playerDistance = Vector3.Distance(transform.position, playerTransform.position);

        // If within range, start shooting at the player
        if (playerDistance <= shootingRange && !shootCooldown)
        {
            navMeshAgent.isStopped = true;
            characterMovement.Animator.SetBool("Aiming", true);
            shootCooldown = true;
            StartCoroutine(nameof(ShootDelay));
        }
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootingDelay);
        if (state == EnemyState.Hunting)
        {
            playerTransform.GetComponent<Health>().Damage(1.0f);
        }
        shootCooldown = false;
    }

    private bool CanSeePlayer()
    {
		Vector3 vectorToPlayer = (playerTransform.position - transform.position);
		Vector3 directionToPlayer = vectorToPlayer.normalized;
		float distanceToPlayer = vectorToPlayer.magnitude;
		// If player is close enough, do the next steps to see if we should become alerted
		if (distanceToPlayer < alertMaxDistance)
		{
			// Check if within field of view (by rearranging dot product formula for angle)
			float angle = Mathf.Acos(Vector3.Dot(transform.forward, directionToPlayer));
			if (angle < alertFieldOfView * Mathf.Deg2Rad / 2.0f)
			{
                Debug.DrawRay(transform.position + visionOffset, directionToPlayer * alertMaxDistance, state == EnemyState.Patrolling ? Color.green : Color.red, 0.01f, true);
                // Now check with a raycast if there are obstacles blocking the view
                if (Physics.Raycast(transform.position + visionOffset + transform.forward, directionToPlayer, out RaycastHit hitInfo, alertMaxDistance))
                {
                    if (hitInfo.transform.CompareTag("Player"))
                    {
                        return true;
                    }
                    else if (hitInfo.transform.parent && hitInfo.transform.parent.CompareTag("Player"))
                    {
                        return true;
                    }
                }
			}
		}
        return false;
	}

    private void SelectNewWaypoint()
    {
        if (state != EnemyState.Patrolling)
            return;

        pausePatrolling = false;
		waypointIndex++;
		if (waypointIndex >= waypoints.Count)
		{
			waypointIndex = 0;
		}
		navMeshAgent.destination = waypoints[waypointIndex].position;
	}

    private void SelectClosestWaypoint()
    {
		float closestDistance = Vector3.Distance(transform.position, waypoints[0].position);
		int closestIndex = 0;
		for (int i = 1; i < waypoints.Count; i++)
		{
			float distance = Vector3.Distance(transform.position, waypoints[i].position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestIndex = i;
			}
		}
		waypointIndex = closestIndex;
        navMeshAgent.destination = waypoints[waypointIndex].position;
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * alertMaxDistance);
        Gizmos.DrawWireSphere(transform.position + visionOffset, 0.1f);
    }
}
