using System;


[Serializable]
public class PFAreaDamageData
{
    public float maxDamage;
    public float maxBlastRange;

    //The number of times this object can collider before exploding
    //1 = rocket like behaviour - explodes on 1st impact
    public int numImpactsToDetonate;
}
