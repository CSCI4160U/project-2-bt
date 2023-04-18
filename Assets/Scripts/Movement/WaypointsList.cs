using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsList : MonoBehaviour
{
    private Transform[] children;

    private void OnDrawGizmosSelected()
    {
        children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            Gizmos.DrawWireCube(child.position + new Vector3(0.0f, 0.25f, 0.0f), Vector3.one / 2.0f);
        }
    }
}
