using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveStateListener : MonoBehaviour
{
    [SerializeField] private ObjectiveSystem objectiveSystem;
    [SerializeField] private int requiredState;
    [SerializeField] private float delay = 0.0f;
    [SerializeField] private UnityEvent onStateMatched;

    private void Start()
    {
        objectiveSystem.OnStateChanged.AddListener(StateChanged);
    }

    private void StateChanged(int state)
    {
		if (state == requiredState)
		{
            Invoke(nameof(SendEvent), delay);
		}
	}

    private void SendEvent()
    {
		onStateMatched.Invoke();
	}
}
