using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleInteractable : InteractableObject
{
    [SerializeField] private string tooltipText;
    [SerializeField] private UnityEvent onInteract;

    public UnityEvent OnInteract { get => onInteract; }

    public override void Interact()
    {
        onInteract.Invoke();
    }

    public override string GetTooltipText()
    {
        return tooltipText;
    }
}
