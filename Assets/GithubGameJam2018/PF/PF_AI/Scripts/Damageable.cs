using System;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    //Notable Events
    public Action OnDamaged;
    public Action OnDestroyed;
    //end

    public float maxHealth;
    [HideInInspector]
    public float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float amount)
    {
        if(OnDamaged != null)
        {
            OnDamaged();

            //on damaged code goes here
        }

        this.currentHealth -= amount;

        if(currentHealth <= 0)
        {
            if (OnDestroyed != null)
            {
                OnDestroyed();

                //on damaged code goes here
            }

            //NB audio source will be destoryed next frame! Spawn new object with audio 
            Destroy(gameObject);
        }
    }
    
}