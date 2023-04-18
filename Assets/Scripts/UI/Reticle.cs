using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    public void AimingChanged(bool aiming)
    {
        gameObject.SetActive(aiming);
    }
}
