using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [SerializeField] private float alertRange = 10.0f;

    [ContextMenu("Explode")]
    public void Explode()
    {
        Destroy(gameObject);

        foreach (Collider collider in Physics.OverlapSphere(transform.position, alertRange, LayerMask.GetMask("Enemy")))
        {
            if (collider.TryGetComponent(out EnemyAI enemy))
            {
                enemy.AlertToPosition(transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }
}
