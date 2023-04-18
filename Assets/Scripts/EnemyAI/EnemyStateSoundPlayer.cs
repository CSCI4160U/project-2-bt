using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class EnemyStateSoundPlayer : MonoBehaviour
{
    [System.Serializable]
    public class Entry
    {
        public EnemyAI.EnemyState State;
        public AudioSource Sound;
    }

    [SerializeField] private List<Entry> sounds;

    private EnemyAI enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyAI>();
        enemy.OnStateChanged.AddListener(StateChanged);
    }

    private void StateChanged(EnemyAI.EnemyState state)
    {
        foreach (Entry entry in sounds)
        {
            if (state == entry.State)
            {
                entry.Sound.Play();
            }
        }
    }
}
