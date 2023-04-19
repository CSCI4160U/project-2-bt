using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [SerializeField] private float alertRange = 10.0f;
    [SerializeField] private AudioSource sound;
    [SerializeField] private new ParticleSystem particleSystem;

    private bool exploding = false;

    [ContextMenu("Explode")]
    public void Explode()
    {
        if (!exploding)
        {
            exploding = true;
			StartCoroutine(nameof(DoExplode));
		}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }

    private IEnumerator DoExplode()
    {
        particleSystem.Play();
        sound.Play();
        GetComponent<Renderer>().enabled = false;
		foreach (Collider collider in Physics.OverlapSphere(transform.position, alertRange, LayerMask.GetMask("Enemy")))
		{
            Debug.Log(collider.gameObject.name);
			if (collider.TryGetComponent(out EnemyAI enemy))
			{
				enemy.AlertToPosition(transform.position);
			}
		}

		yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }
}
