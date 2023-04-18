using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIndicator : MonoBehaviour
{
    [SerializeField] private GameObject alertIndicator;
    [SerializeField] private GameObject HuntIndicator;
	[SerializeField] private GameObject minimapObject;
	[SerializeField] private Material patrollingMaterial;
	[SerializeField] private Material alertMaterial;
	[SerializeField] private Material huntingMaterial;

	private Renderer minimapObjectRenderer;

	private void Start()
	{
		minimapObjectRenderer = minimapObject.GetComponent<Renderer>();
	}

	public void UpdateIndicators(EnemyAI.EnemyState state)
    {
		alertIndicator.SetActive(false);
		HuntIndicator.SetActive(false);
		if (state == EnemyAI.EnemyState.Alerted)
		{
			alertIndicator.SetActive(true);
			minimapObjectRenderer.material = alertMaterial;
			
		}
		else if (state == EnemyAI.EnemyState.Hunting)
		{
			HuntIndicator.SetActive(true);
			minimapObjectRenderer.material = huntingMaterial;
		}
		else
		{
			minimapObjectRenderer.material = patrollingMaterial;
		}
	}
}
