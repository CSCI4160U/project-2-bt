using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Tooltips tooltips;
    [SerializeField] private float range = 3.0f;
    [SerializeField] private Vector3 interactionOrigin = Vector3.zero;

    private LayerMask layerMask;
    private List<InteractableObject> interactablesInRange = new List<InteractableObject>();

    private Dictionary<string, string> buttonsToKeys = new Dictionary<string, string>()
    {
        { "Interaction1", "E" },
        { "Interaction2", "F" },
    };

    private void OnEnable()
    {
		StartCoroutine(nameof(InteractionLoop));
	}

    private void OnDisable()
    {
        StopCoroutine(nameof(InteractionLoop));
    }

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        foreach (InteractableObject interactable in interactablesInRange)
        {
            if (interactable.enabled && Input.GetButtonDown(interactable.ButtonName))
            {
                interactable.Interact();
            }
        }
    }

    private IEnumerator InteractionLoop()
    {
        while (true)
        {
            interactablesInRange.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position + interactionOrigin, range, layerMask);
            foreach (Collider collider in colliders)
            {
                InteractableObject interactable = collider.GetComponentInParent<InteractableObject>();
                if (interactable != null)
                {
                    interactablesInRange.Add(interactable);
                }
            }

			tooltips.Clear();
			foreach (InteractableObject interactable in interactablesInRange)
			{
                string key = interactable.ButtonName;
                if (buttonsToKeys.ContainsKey(key))
                {
                    key = buttonsToKeys[key];
                }
                tooltips.Add($"{key}: {interactable.GetTooltipText()}");
			}

			yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + interactionOrigin, range);
    }
}
