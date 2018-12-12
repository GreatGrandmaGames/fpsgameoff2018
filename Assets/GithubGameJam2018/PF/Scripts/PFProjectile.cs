using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PFProjectile : MonoBehaviour {

    //Notable Events:
    public Action OnCollideWithSurface;
    public Action OnImpact;
    public Action OnExplode;
    //end


    private const float minSpeedBeforeDetonation = 0.001f;

    //Calculated properties
    private float timeSinceCreation = 0.0f;
    private float distanceTravelled = 0.0f;
    private Vector3? previousPosition = null; //used for previousPosition
    private int numberOfCollisions; //used for explosion

    //Component References
    Rigidbody rb;

    //Data Properties
    private PFProjectileData data;
    public PFProjectileData Data {
        get
        {
            return data;
        }
        set
        {
            data = value;

            GetComponent<MeshRenderer>().enabled = true;

            ApplyStartingTrajectory();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        GetComponent<MeshRenderer>().enabled = false;

        //Clean up
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if(data == null)
        {
            //not ready to start moving
            return;
        }

        //Motion
        AddAdditionalForces(timeSinceCreation);

        //When the prjectile stops moving, explode
        //if(rb.velocity.magnitude < minSpeedBeforeDetonation)
        //{
            //Debug.Log(rb.velocity.magnitude);
            //Explode();
        //}

        //Frame admin
        //Record travel time
        timeSinceCreation += Time.fixedDeltaTime;

        //Record distance travelled
        if (previousPosition.HasValue)
        {
            distanceTravelled += Vector3.Distance(previousPosition.Value, transform.position);
        }

        previousPosition = transform.position;
    }

   private void OnCollisionEnter(Collision collision)
    {        

        if(OnCollideWithSurface != null)
        {
            OnCollideWithSurface.Invoke();

            //OnCollideWithSurface code goes here
        }

        numberOfCollisions++;

        var hitMax = numberOfCollisions >= Data.AreaDamage.numImpactsToDetonate;
        var damageable = collision.transform.GetComponent<Damageable>();

        if (damageable != null)
        {
            Impact(damageable);
            Explode();
        }
        else if (hitMax)
        {
            Explode();
        }
    }

    #region Motion
    private void ApplyStartingTrajectory()
    {
        //Generate any projectile specific random values
        float trajectoryScalarX = RandomUtility.RandFloat(-1f, 1f);
        float trajectoryScalarY = RandomUtility.RandFloat(-1f, 1f);

        Vector3 v = transform.forward;

        v += transform.right * Mathf.Tan(trajectoryScalarX) * Data.Trajectory.maxInitialSpreadAngle;
        v += transform.up * Mathf.Tan(trajectoryScalarY) * Data.Trajectory.maxInitialSpreadAngle;

        v = v.normalized * Data.Trajectory.initialSpeed;

        rb.AddForce(v, ForceMode.Impulse);
    }

    private void AddAdditionalForces(float time)
    {
        //Gravity
        //WHy do we multiply by initialSpeed here?
        //If we just applied some drop off force, this would be a NON-ORTHONGONAL feature
        //because the faster your initial speed, the less dropoff has time to affect the course 
        //of the projetile.
        //Here, the initial speed is included so that no matter how fast the projectile travels,
        //the same dropoff ratio for one gun is the same for another (with a different initial speed)
        rb.AddForce(Vector3.down * Data.Trajectory.dropOffRatio * Data.Trajectory.initialSpeed / Time.fixedDeltaTime);

        //Bullet curve, heat seeking etc.
    }

    #endregion

    #region Impact
    /// <summary>
    /// When a projectile impacts some surface
    /// </summary>
    private void Impact(Damageable damagable)
    {
        if (damagable == null)
        {
            return;
        }

        damagable.Damage(CalculateDamageOnImpact());

        if (OnImpact != null)
        {
            OnImpact.Invoke();

            //OnCollideWithSurface code goes here
        }
    }


    /// <summary>
    /// Uses PGFImpaceDamageData to calculate impace damage. This is non-explosive damage
    /// e.g. piercing, blunt force
    /// </summary>
    /// <returns></returns>
    public float CalculateDamageOnImpact() {

        return Data.ImpactDamage.baseDamage + Data.ImpactDamage.damageDropOff * distanceTravelled;
    }
    #endregion

    #region Area
    public void Explode()
    {
        foreach (var col in Physics.OverlapSphere(transform.position, Data.AreaDamage.maxBlastRange))
        {
            var dam = col.GetComponent<Damageable>();

            if (dam != null)
            {
                dam.Damage(CalculateDamageOnExplosion(dam.transform.position));
            }
        }

        if (OnExplode != null)
        {
            OnExplode.Invoke();

            //OnCollideWithSurface code goes here
        }

        //NB audio source will be destoryed next frame! Spawn new object with audio source 
        Destroy(gameObject);
    }

    private float CalculateDamageOnExplosion(Vector3 otherPosition)
    {
        float dist = Vector3.Distance(transform.position, otherPosition);

        if(dist < Data.AreaDamage.maxBlastRange)
        {
            return (dist / Data.AreaDamage.maxBlastRange) * Data.AreaDamage.maxDamage;
        } else
        {
            return 0f;
        }
    }
    #endregion
}
