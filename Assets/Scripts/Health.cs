using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth = 10.0f;
    [SerializeField] private float maxHealth = 10.0f;
    [SerializeField] private UnityEvent<float> onCurrentHealthChanged;
    [SerializeField] private UnityEvent onKilled;
    [SerializeField] private Slider uiSlider;

    public float CurrentHealth { get => currentHealth; }
    public float MaxHealth { get => maxHealth; }
    public UnityEvent<float> OnCurrentHealthChanged { get => onCurrentHealthChanged; }
    public UnityEvent OnKilled { get => onKilled; }

    public void Damage(float delta)
    {
        currentHealth = Mathf.Max(currentHealth - delta, 0.0f);

        onCurrentHealthChanged.Invoke(currentHealth);

        if (currentHealth <= 0.0f)
        {
            onKilled.Invoke();
        }

        if (uiSlider != null)
        {
            uiSlider.value = currentHealth / maxHealth;
        }
    }

    public void Heal(float delta)
    {
        currentHealth = Mathf.Min(currentHealth + delta, maxHealth);

		onCurrentHealthChanged.Invoke(currentHealth);

		if (uiSlider != null)
		{
			uiSlider.value = currentHealth / maxHealth;
		}
	}

    [ContextMenu("Kill")]
    public void Kill()
    {
        Damage(maxHealth);
    }

    public void Load(float health)
    {
        currentHealth = health;

		onCurrentHealthChanged.Invoke(currentHealth);

		if (uiSlider != null)
		{
			uiSlider.value = currentHealth / maxHealth;
		}
	}

    private void OnValidate()
    {
		currentHealth = maxHealth;
    }
}
