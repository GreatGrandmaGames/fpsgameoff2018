using System;
using System.Collections;
using UnityEngine;


public class ParametricFirearm : MonoBehaviour
{
    [Header("Projectile Spawning")]
    public GameObject projectilePrefab;
    [Tooltip("Where the projectile will spawn from and its initial direction (z-axis)")]
    public Transform barrelTip;

    //Data Properties
    //Generated and set by the factory
    public PFData Data;

    //Ammo remaining in the clip
    public int CurrentAmmo { get; private set; }

    //Bookkeeping
    public enum PFState
    {
        Ready,
        Charging,
        CoolDown,
        CoolDownInterupt,
        ManualReload
    }

    private PFState state;

    public PFState State {
        get
        {
            return state;
        }
        private set
        {
            state = value;

            if(onStateChanged != null)
            {
                onStateChanged(value);
            }
        }
    }

    public Action<PFState> onStateChanged;

    private Coroutine chargeCoroutine;
    private Coroutine manaualReloadCoroutine;
    private Coroutine coolDownCoroutine;

    private void Start()
    {
        CurrentAmmo = Data.RateOfFire.AmmoCapacity;
    }

    #region Public Methods
    /// <summary>
    /// When in Ready state, will begin charging the weapon. NB if chargeTime is 0, will immediately call fire
    /// </summary>
    public Coroutine TriggerPress()
    {
        if (State == PFState.Ready)
        {
            State = PFState.Charging;
            chargeCoroutine = StartCoroutine(Charge());

            return chargeCoroutine;
        }

        return null;
    }

    /// <summary>
    /// If in Charging state, will either stop charging or fire depending on Data
    /// </summary>
    public Coroutine TriggerRelease()
    {
        if (State == PFState.Charging)
        {
            //Interupt charging
            StopCoroutine(chargeCoroutine);

            if (Data.ChargeTime.releaseBeforeChargeComplete)
            {
                //Fire
                return Fire();
            } else
            {
                //Cancel
                State = PFState.Ready;
            }
        }

        return null;
    }

    /// <summary>
    /// If in Ready or Charging, will begin a manual reload
    /// </summary>
    public void ManualReload()
    {
        if (State == PFState.Ready || State == PFState.Charging)
        {
            if (chargeCoroutine != null)
            {
                StopCoroutine(chargeCoroutine);
            }

            manaualReloadCoroutine = StartCoroutine(Reload());
        }
    }

    /// <summary>
    /// If ManualReload, will switch back to ready
    /// </summary>
    public void CancelManualReload()
    {
        StopCoroutine(manaualReloadCoroutine);
        State = PFState.Ready;
    }

    public void ResumeCoolDown()
    {
        if(State == PFState.CoolDownInterupt)
        {
            State = PFState.CoolDown;
            coolDownCoroutine = StartCoroutine(CoolDown());
        }
    }

    public void InteruptCoolDown()
    {
        if(State == PFState.CoolDown)
        {
            State = PFState.CoolDownInterupt;
            StopCoroutine(coolDownCoroutine);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Launches projectile(s) and transistions into cool down
    /// </summary>    
    private Coroutine Fire()
    {
        for(int i = 0; i < Data.Multishot.numberOfShots; i++)
        {
            //Spawn the projectile
            var projectileObject = Instantiate(projectilePrefab);

            projectileObject.transform.position = barrelTip.position;
            projectileObject.transform.forward = barrelTip.forward;


            var projectileComponent = projectileObject.GetComponent<PFProjectile>();

            //set projectile data
            projectileComponent.Data = Data.Projectile;

            //Controlling ROF
            //CUrrent ammo is decremented before being sent to GetWaitTime to avoid the off by one error
            CurrentAmmo--;

            //Run out of ammo - will force reload
            if(CurrentAmmo <= 0)
            {
                break;
            }
        }

        coolDownCoroutine = StartCoroutine(CoolDown());

        return coolDownCoroutine;
    }


    private IEnumerator Charge()
    {
        //state is charge
        yield return new WaitForSeconds(Data.ChargeTime.chargeTime);

        Fire();
        //state is cool down
    }

    /// <summary>
    /// Prevents the PF for firing. Used to control rate of fire and forced reloading
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator CoolDown()
    {
        State = PFState.CoolDown;
        yield return new WaitForSeconds(Data.RateOfFire.GetWaitTime(CurrentAmmo));

        //If was a forced reload
        if(CurrentAmmo <= 0)
        {
            CurrentAmmo = Data.RateOfFire.AmmoCapacity;
        }

        State = PFState.Ready;
    }


    private IEnumerator Reload()
    {
        State = PFState.ManualReload;
        yield return new WaitForSeconds(Data.RateOfFire.reloadingData.r);

        //for now, we are assuming the Overwatch model of ammo - infinte with reloads
        CurrentAmmo = Data.RateOfFire.AmmoCapacity;
        State = PFState.Ready;
    }
    #endregion

    public override string ToString()
    {
        return string.Format("PF named {0} is in state: {1}, has current ammo {2}", Data.Meta.name, State.ToString(), CurrentAmmo);
    }
}
