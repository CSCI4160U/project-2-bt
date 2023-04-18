using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator animator;
	[SerializeField] private UnityEvent onOpened;
	[SerializeField] private UnityEvent onClosed;

	private bool isOpen = false;

    public bool IsOpen { get => isOpen; }
    public UnityEvent OnOpened { get => onOpened; }
    public UnityEvent OnClosed { get => onClosed; }

    [ContextMenu("Open")]
    public void Open()
    {
        if (isOpen)
            return;

		isOpen = true;
        animator.SetTrigger("Open");
        onOpened.Invoke();
    }

    [ContextMenu("Close")]
    public void Close()
    {
        if (!isOpen)
            return;

        isOpen = false;
        animator.SetTrigger("Close");
		onClosed.Invoke();
	}

    [ContextMenu("Toggle")]
    public void Toggle()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
