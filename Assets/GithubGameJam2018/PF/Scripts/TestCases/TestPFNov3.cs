using System;
using System.Collections.Generic;
using UnityEngine;

public class TestPFNov3 : MonoBehaviour {

    public KeyCode fire;
    public KeyCode reload;
    public KeyCode fakeSwitchWeapon;

    public Transform pfTransform;

    private ParametricFirearm pf;

    private void Start()
    {
        var pfData = new PFData()
        {
            Projectile = new PFProjectileData()
            {
                Trajectory = new PFTrajectoryData()
                {
                    initialSpeed = 50f,
                    dropOffRatio = 0.1f,
                    maxInitialSpreadAngle = 0.1f
                },
            },
            RateOfFire = new PFRateOfFireData(10, 1f)
            {
                baseRate = 0.25f,
            },
            Multishot = new PFMultishotData()
            {
                numberOfShots = 2,
            },
            ChargeTime = new PFChargeTimeData()
            {
                releaseBeforeChargeComplete = true,
                chargeTime = 0.5f
            }
        };

        pf = PFFactory.Instance.CreatePF(pfData);

        pf.transform.SetParent(pfTransform);

        pf.onStateChanged += (state) =>
        {
            Debug.Log(pf);
        };
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Nov3: Fire key pressed");
            pf.TriggerPress();
        }

        if (Input.GetKeyUp(fire))
        {
            Debug.Log("Nov3: Fire key released");
            pf.TriggerRelease();
        }

        if (Input.GetKeyDown(reload))
        {
            if (pf.State == ParametricFirearm.PFState.ManualReload)
            {
                pf.CancelManualReload();
            }
            else
            {
                pf.ManualReload();
            }
        }

        if (Input.GetKeyDown(fakeSwitchWeapon))
        {
            if (pf.State == ParametricFirearm.PFState.CoolDown)
            {
                pf.InteruptCoolDown();
            }
            else if (pf.State == ParametricFirearm.PFState.CoolDownInterupt)
            {
                pf.ResumeCoolDown();
            }
        }
    }
}
