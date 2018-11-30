using System;
using System.Collections;
using UnityEngine;

using UnityEngine.AI;

/// <summary>
/// An AI Agent that wields a Parametric Firearm
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Damageable))]
public class PF_AI : MonoBehaviour {

    public Transform pfParent;
    public ParametricFirearm pfPrefab;

    private NavMeshAgent agent;
    private ParametricFirearm pf;
    private Transform referenceBarrelTransform;

    public enum AIState
    {
        Idle, //Stationary
        Intercepting,
        Firing,
    }

    private AIState state;
    private AIState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            Debug.Log(this);
        }
    }

    public PF_AIData Data;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        //PF Init
        pf = Instantiate(pfPrefab, pfParent).GetComponent<ParametricFirearm>();

        pf.transform.localPosition = Vector3.zero;
        pf.transform.localRotation = Quaternion.identity;
        pf.transform.localScale = Vector3.one;

        referenceBarrelTransform = new GameObject("Reference Barrel Transform").transform;

        //Should not move with the parent
        referenceBarrelTransform.SetParent(pfParent.parent);
        referenceBarrelTransform.transform.position = pf.barrelTip.transform.position;
        referenceBarrelTransform.transform.rotation = pf.barrelTip.transform.rotation;
    }

    private void Start()
    {
        //Movement Speed changes established
        pf.onStateChanged += (state) =>
        {
            //State transitions to charging - firing begins
            if (state == ParametricFirearm.PFState.Charging)
            {
                //Modify movement by firing move speed
                agent.speed = Data.moveSpeed * Data.firingMoveSpeedFactor;
            }
            //State transitions to ready - firing finished or cancelled
            else if (state == ParametricFirearm.PFState.Ready)
            {
                //Restore normal speed
                agent.speed = Data.moveSpeed;
            }
        };
    }

    public void SetTarget(Damageable d)
    {
        StartCoroutine(Intercept(d));
    }

    private IEnumerator Intercept(Damageable d)
    {
        State = AIState.Intercepting;

        agent.isStopped = false;

        while (TargetInSight(d) == false || TargetInRange(d.transform) == false)
        {
            Debug.Log("Set dest: " + d.transform.position);
            agent.SetDestination(d.transform.position);

            transform.LookAt(d.transform.position, Vector3.up);

            yield return null;         
        }

        if(d != null)
        {
            //Target in sight, begin attack
            StartCoroutine(Firing(d));
        } else
        {
            //case: Target Dead
            State = AIState.Idle;

            agent.isStopped = true;
        }
    }

    private IEnumerator Firing(Damageable d)
    {
        State = AIState.Firing;

        agent.isStopped = true;

        var aim = StartCoroutine(Aim(d.transform));

        while (TargetInSight(d) && TargetInRange(d.transform))
        {
            //TODO: does this work if StopCoroutine is called?
            yield return pf.TriggerPress();

            yield return pf.TriggerRelease();
        }

        StopCoroutine(aim);

        //Target escaped case
        if(d != null)
        {
            StartCoroutine(Intercept(d));
        } else
        //Target dead case
        {
            State = AIState.Idle;
        }
    }

    private IEnumerator Aim(Transform d)
    {
        //Controlled by Firing
        while (true)
        {
            //Turn body towards planar point of target
            transform.LookAt(new Vector3(d.position.x, transform.position.y, d.position.z));

            if (TargetInRange(d))
            {
                pfParent.localEulerAngles = new Vector3(-CalculatePFWeaponAngle(d), 0, 0);
            }

            yield return null;
        }
    }

    private bool TargetInSight(Damageable d)
    {
        if(d == null)
        {
            return false;
        }

        //Measured from pf barrel tip
        Vector3 startPos = referenceBarrelTransform.position;
        Vector3 endPos = d.transform.position;
        //float maxRange = CalculateMaxPFRange(d.transform);

        RaycastHit info;

        if(Physics.Raycast(startPos, (endPos - startPos), out info, Mathf.Infinity))
        {
            var d1 = info.collider.GetComponentInParent<Damageable>();

            return d1 == d;
        }

        return false;
    }

    private bool TargetInRange(Transform d)
    {
        return float.IsNaN(CalculatePFWeaponAngle(d)) == false;
    }

    /*
    /// <summary>
    /// Calculate the maximum possible range (ie, when angle = 45)
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    private float CalculateMaxPFRange(Transform d)
    {
        //Initial velocity
        float v0 = pf.Data.Projectile.Trajectory.initialSpeed;
        //Gravity (acceleration)
        float g = (pf.Data.Projectile.Trajectory.dropOffRatio * pf.Data.Projectile.Trajectory.initialSpeed) / Time.fixedDeltaTime;
        //Height difference
        float h = referenceBarrelTransform.position.y - d.position.y;

        float sinTheta = Mathf.Sin(Mathf.PI / 4);
        float cosTheta = Mathf.Cos(Mathf.PI / 4); //Sqrt(2) / 2

        float b = v0 * sinTheta;

        float t0 = (b + Mathf.Sqrt((b * b) + (2 * g * h))) / g;
        float t1 = (b - Mathf.Sqrt((b * b) + (2 * g * h))) / g;

        float t = Mathf.Max(t0, t1);

        return t * v0 * cosTheta;
    }
    */


    /// <summary>
    /// Calculates the lowest firing angle to reach the target
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    private float CalculatePFWeaponAngle(Transform d)
    {
        //Initial velocity
        float v0 = pf.Data.Projectile.Trajectory.initialSpeed;
        //Gravity (acceleration)
        float g = (pf.Data.Projectile.Trajectory.dropOffRatio * pf.Data.Projectile.Trajectory.initialSpeed) / Time.fixedDeltaTime;
        //Height difference
        float sy = referenceBarrelTransform.position.y - d.position.y;
        //Distance
        float sx = referenceBarrelTransform.position.x - d.position.x;
        float sz = referenceBarrelTransform.position.z - d.position.z;

        //planar distance
        float p = Mathf.Sqrt(sx * sx + sz * sz);

        float q = -((g * p * p) / (2 * v0 * v0));

        float tanTheata0 = (-p + Mathf.Sqrt((p * p) - (4 * q * (q + sy)))) / (2 * q);
        float tanTheata1 = (-p - Mathf.Sqrt((p * p) - (4 * q * (q + sy)))) / (2 * q);

        float theta0 = Mathf.Max(0, Mathf.Atan(tanTheata0)) * (180f / Mathf.PI);
        float theta1 = Mathf.Max(0, Mathf.Atan(tanTheata1)) * (180f / Mathf.PI);

        return Mathf.Min(theta0, theta1);
    }

    public override string ToString()
    {
        return string.Format("PF_AI: In state: {0}", State);
    }
}

[Serializable]
public class PF_AIData
{
    public float moveSpeed;
    public float firingMoveSpeedFactor;
}
