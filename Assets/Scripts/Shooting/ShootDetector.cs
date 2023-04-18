using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent onShoot;

    public UnityEvent OnShoot { get => onShoot; }
}
