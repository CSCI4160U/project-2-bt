using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnKilled : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnKilled.AddListener(ForceDestroy);
    }

    [ContextMenu("Force Destroy")]
    public void ForceDestroy()
    {
        Destroy(gameObject);
    }
}
