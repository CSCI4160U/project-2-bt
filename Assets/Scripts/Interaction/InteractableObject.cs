using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{    
    [Tooltip("The name of the button which would go into a Input.GetButtonDown() call")]
    [SerializeField] protected string buttonName = "Interaction1";

    public string ButtonName { get => buttonName; }

    public abstract void Interact();
	public abstract string GetTooltipText();
}
