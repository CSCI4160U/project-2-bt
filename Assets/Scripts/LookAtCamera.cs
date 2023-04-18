using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [Tooltip("The camera to look at, automatically set to the main camera if left empty")]
    [SerializeField] private Camera targetCamera;

    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
