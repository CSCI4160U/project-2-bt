using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private AudioSource sound;

    public void Fire()
    {
        sound.Play();
    }
}
