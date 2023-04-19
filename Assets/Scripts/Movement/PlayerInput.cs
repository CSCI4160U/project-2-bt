using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Health))]
public class PlayerInput : MonoBehaviour, ISaveable
{
    [SerializeField] private Transform spine;
    [SerializeField] private Gun gun;
    [SerializeField] private LayerMask shootLayerMask;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private UnityEvent onShoot;

    private CharacterMovement character;
    private float spineRotation = 0.0f;
    private Camera mainCamera;

    public float SpineRotation { get => spineRotation; }
    public UnityEvent OnShoot { get => onShoot; }

    private void Start()
    {
        character = GetComponent<CharacterMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
	}

    private void Update()
    {
        if (pauseMenu.Paused)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Running
        if (Input.GetKeyDown(KeyCode.LeftShift))
            character.Running = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            character.Running = false;

        // Sneaking
        if (Input.GetKeyDown(KeyCode.LeftControl))
            character.Sneaking = !character.Sneaking;

        // Aiming
        if (Input.GetMouseButtonDown(1))
            character.Aiming = true;
        if (Input.GetMouseButtonUp(1))
            character.Aiming = false;

        // Movement
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        character.Move(movement);

        // Rotation
        float rotationX = Input.GetAxis("Mouse X");
        character.Rotate(rotationX);

        float rotationY = Input.GetAxis("Mouse Y");
        spine.localRotation *= Quaternion.Euler(new Vector3(rotationY, 0.0f, 0.0f));

        // Shooting
        if (Input.GetMouseButtonDown(0) && character.Aiming)
        {
            gun.Fire();
            onShoot.Invoke();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 40.0f, shootLayerMask))
            {
                if (hit.transform.TryGetComponent(out ShootDetector det))
                {
                    det.OnShoot.Invoke();
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (pauseMenu.Paused)
            return;

		float rotationY = Input.GetAxis("Mouse Y");
        float oldSpineRotation = spineRotation;
        spineRotation -= rotationY * 2.0f;
        spineRotation = Mathf.Clamp(spineRotation, -45.0f, 45.0f);
        float delta = spineRotation - oldSpineRotation;
		spine.localRotation *= Quaternion.Euler(new Vector3(spineRotation, 0.0f, 0.0f));
        mainCamera.transform.RotateAround(spine.transform.position, transform.right, delta);
	}

    [System.Serializable]
    private struct SaveData
    {
        public float SpineRotation;
        public Vector3 Position;
        public Vector3 Rotation;
        public float Health;
    }

    public void Save(string savePath)
    {
        SaveData data = new SaveData()
        {
            SpineRotation = spineRotation,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles,
            Health = GetComponent<Health>().CurrentHealth,
        };
        File.WriteAllText(savePath + "Player.json", JsonUtility.ToJson(data));
    }

    public void Load(string savePath)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath + "Player.json"));
        spineRotation = data.SpineRotation;
        transform.position = data.Position;
        transform.rotation = Quaternion.Euler(data.Rotation);
        GetComponent<Health>().Load(data.Health);
    }
}
