using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    //added stuff
    //TODO: Damageable data?
    public float StartingHealth;

    private float currentHealth;

	public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            currentHealth = value;

            if (currentHealth < 0)
            {
                Destroyed();
            }
        }
    }

    private void Awake()
    {
        CurrentHealth = StartingHealth;
    }

    /// <summary>
    /// Calls Destory(gameObject). Use with caution
    /// </summary>
    protected virtual void Destroyed()
    {
        Destroy(gameObject);
    }

}
