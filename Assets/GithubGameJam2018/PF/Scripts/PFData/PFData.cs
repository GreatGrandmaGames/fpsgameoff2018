using System;
using UnityEngine;

[Serializable]
public class PFData
{
    [SerializeField]
    public PFMetaData Meta;
    [SerializeField]
    public PFProjectileData Projectile;
    [SerializeField]
    public PFRateOfFireData RateOfFire;
    [SerializeField]
    public PFMultishotData Multishot;
    [SerializeField]
    public PFChargeTimeData ChargeTime;

    public PFData()
    {
        this.Meta = new PFMetaData();
        this.Projectile = new PFProjectileData();
        this.RateOfFire = new PFRateOfFireData();
        this.Multishot = new PFMultishotData();
        this.ChargeTime = new PFChargeTimeData();
    }
}

[Serializable]
public class PFProjectileData
{
    [SerializeField]
    public PFImpactDamageData ImpactDamage;
    [SerializeField]
    public PFAreaDamageData AreaDamage;
    [SerializeField]
    public PFTrajectoryData Trajectory;

    public PFProjectileData()
    {
        this.ImpactDamage = new PFImpactDamageData();
        this.AreaDamage = new PFAreaDamageData();
        this.Trajectory = new PFTrajectoryData();
    }
}

[Serializable]
public class PFMetaData
{
    public string name;
    public string type;

    public PFMetaData()
    {
        name = PFRandomNames.GenerateName();
        //type is currently unused
    }
}