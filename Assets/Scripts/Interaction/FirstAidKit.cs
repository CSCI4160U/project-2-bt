using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKit : MonoBehaviour
{
    [SerializeField] private int healAmount = 5;

    public int HealAmount { get => healAmount; }

    public void Use()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().Heal(healAmount);
        gameObject.SetActive(false);
    }
}
