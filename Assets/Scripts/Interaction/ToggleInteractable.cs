using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleInteractable : InteractableObject
{
    [SerializeField] private string trueTooltipText = "True";
    [SerializeField] private string falseTooltipText = "False";
    [SerializeField] private UnityEvent onInteract;

    private bool state = false;

    public UnityEvent OnInteract { get => onInteract; }

    public override void Interact()
    {
        onInteract.Invoke();
    }

    public void SetState(bool state)
    {
        this.state = state;
    }

    public override string GetTooltipText()
    {
        return state ? trueTooltipText : falseTooltipText;
    }
}
