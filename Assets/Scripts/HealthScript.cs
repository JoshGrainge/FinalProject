using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HealthScript : MonoBehaviour
{
    public float maxHealth = 100;

    public float health;

    public delegate void ExecuteDamage(float damage);
    public event ExecuteDamage OnDamage;

    // Initially subscribe to remove health function
    private void Start()
    {
        OnDamage += RemoveHealth;
    }

    /// <summary>
    /// Calls function to deal damage and play hit audio on characters
    /// </summary>
    /// <param name="damage">Amount of damage to deal to character</param>
    public void DealDamage(float damage)
    {
        OnDamage(damage);
    }

    /// <summary>
    /// Removes health from character
    /// </summary>
    /// <param name="damage">Amount of damage to deal to character</param>
    void RemoveHealth(float damage)
    {
        health -= damage;
    }




    /// <summary>
    /// Heal character value amount, with range of 0 to characters max health
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(float healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, maxHealth);
    }
}
