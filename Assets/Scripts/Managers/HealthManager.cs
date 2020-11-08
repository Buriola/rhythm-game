using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control the health state of the player
/// </summary>
public class HealthManager : Singleton<HealthManager>
{
	public bool Failed { get; private set; } //Bool to represent if the bar reached 0

    [SerializeField]
    [Range(100, 200)]
    private float maxHealth; //Max Health
    private float currentHealth; //Currente Health

    //Events for UI subscription or whoelse is interested to know this
    [SerializeField]
    private UnityEngine.Events.UnityEvent onHealthChange;
    [SerializeField]
    private UnityEngine.Events.UnityEvent onHealthReachingZero;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Adds HP
    /// </summary>
    /// <param name="amount">The amount you want to add</param>
    public void AddHP(float amount)
    {
        if (Failed) return; //Ignores if it alrealdy reached 0

        //Magic math
        currentHealth += amount;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        onHealthChange.Invoke(); //Call event

    }

    /// <summary>
    /// Removes HP
    /// </summary>
    /// <param name="amount">The amount to remove</param>
    public void DecreaseHP(float amount)
    {
        if (Failed) return; //Ignores if it already reached 0

        //Magic math
        if (currentHealth > 0)
            currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Failed = true;
            onHealthReachingZero.Invoke(); //Call event
        }

        onHealthChange.Invoke(); //Call event
    }

    //Getters bellow...
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

}
