using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PFImpactDamageData
{
    //Variables
    // baseDamage : -1 -> 1 (negative values mean healing)
    [SerializeField]
    public float baseDamage;

    //A rate of damage change over distance travelled
    [SerializeField]
    public float damageDropOff;
}
