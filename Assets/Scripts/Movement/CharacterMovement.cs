using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 4.0f;
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float sneakSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 300.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent<bool> onAimingChanged;

    private new Rigidbody rigidbody;
    private bool sneaking = false;
    private bool running = false;
    private bool aiming = false;
    private Vector3 movement = Vector3.zero;

    public bool Sneaking { get => sneaking; set { sneaking = value; animator.SetBool("Sneaking", value); } }
    public bool Running { get => running; set { running = value; animator.SetBool("Running", value); } }
    public bool Aiming { get => aiming; set { aiming = value; animator.SetBool("Aiming", value); onAimingChanged.Invoke(value); } }
    public float RunSpeed { get => runSpeed; }
    public float WalkSpeed { get => walkSpeed; }
    public float SneakSpeed { get => sneakSpeed; }
    public float RotationSpeed { get => rotationSpeed; }
    public Rigidbody Rigidbody { get => rigidbody; }
    public Animator Animator { get => animator; }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 movement)
    {
        if (movement.magnitude > 1.0f)
        {
            movement = movement.normalized;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);

		float speed = walkSpeed;
		if (sneaking)
		{
			speed = sneakSpeed;
		}
		else if (running)
		{
			speed = runSpeed;
		}
		this.movement = movement * speed;
    }

    public void Rotate(float amount)
    {
        transform.Rotate(0.0f, amount * rotationSpeed * Time.deltaTime, 0.0f);
        animator.SetFloat("Rotation", amount);
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position + (transform.forward * movement.z * Time.fixedDeltaTime + transform.right * movement.x * Time.fixedDeltaTime));
    }
}
