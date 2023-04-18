using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private string messageOnTrigger;

    private ObjectiveSystem objectiveSystem;

    private void Start()
    {
        objectiveSystem = FindObjectOfType<ObjectiveSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objectiveSystem.TriggerDetected(messageOnTrigger);
        }
    }
}
