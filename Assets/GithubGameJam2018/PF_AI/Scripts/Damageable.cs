using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    //added stuff sss
    //TODO: Damageable data?
    public float StartingHealth;
    //hello
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Debug.Log("CURRENT HEALTH: " + currentHealth);
            currentHealth = currentHealth - collision.gameObject.GetComponent<PFProjectile>().CalculateDamageOnImpact();
            if(currentHealth < 0)
            {
                Destroyed();
            }//Destroy(this.gameObject);
        }
    }

}
