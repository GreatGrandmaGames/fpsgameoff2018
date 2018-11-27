using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PFChargeTimeData
{
    public float chargeTime;
    public bool releaseBeforeChargeComplete;

    //Currently unused
    [Tooltip("Current unused")]
    public float damageIncreaseRate;
}

