using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Hostage : MonoBehaviour, ISaveable
{
    [SerializeField] private AudioSource thankYouSound;

    private bool followingPlayer = false;
	private NavMeshAgent navMeshAgent;
	private CharacterMovement characterMovement;
	private Transform playerTransform;

    public bool FollowingPlayer { get => followingPlayer; }

	private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterMovement = GetComponent<CharacterMovement>();

		navMeshAgent.speed = characterMovement.WalkSpeed;
		navMeshAgent.angularSpeed = characterMovement.RotationSpeed;

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!followingPlayer)
            return;

        float playerDistance = Vector3.Distance(transform.position, playerTransform.position);

		if (playerDistance > 8.0f)
		{
			transform.position = playerTransform.position - playerTransform.forward;
		}
		else if (playerDistance > 4.0f)
        {
			navMeshAgent.destination = playerTransform.position;
			navMeshAgent.isStopped = false;
        }
        else if (playerDistance < 2.0f)
        {
            navMeshAgent.isStopped = true;
        }

        Debug.Log(navMeshAgent.velocity);
        characterMovement.Animator.SetFloat("Speed", Mathf.Abs(transform.InverseTransformVector(navMeshAgent.velocity).z));
    }

    [ContextMenu("Start Following Player")]
    public void StartFollowingPlayer()
    {
		Invoke(nameof(AnimationComplete), 6.5f);
		characterMovement.Animator.SetTrigger("Stand Up");
        thankYouSound.Play();
	}

    [System.Serializable]
    private struct SaveData
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public bool FollowingPlayer;
    }

    public void Save(string savePath)
    {
        SaveData data = new SaveData();
        data.Position = transform.position;
        data.Rotation = transform.rotation.eulerAngles;
        data.FollowingPlayer = followingPlayer;

        File.WriteAllText(savePath + "Hostage.json", JsonUtility.ToJson(data));
    }

    public void Load(string savePath)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath + "Hostage.json"));
		if (data.FollowingPlayer)
        {
			transform.position = data.Position;
            transform.rotation = Quaternion.Euler(data.Rotation);
            followingPlayer = true;
            characterMovement.Animator.SetTrigger("Skip");
        }
    }

    private void AnimationComplete()
    {
        followingPlayer = true;
    }
}
