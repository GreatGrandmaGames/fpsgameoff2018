using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PFRateOfFireData
{

    //r0
    public float baseRate;
    //Separated out for clarity, since players can trigger the reload manually
    //and when we call this data, we have to reset the ammo count
    public PFBurstData reloadingData;
    [SerializeField]
    public List<PFBurstData> burstData;

    //This is bookkeeping for GetWaitTime
    [NonSerialized]
    public List<PFBurstData> burstAndReload;

    public int AmmoCapacity
    {
        get
        {
            return reloadingData.n;
        }
    }

    public PFRateOfFireData(int ammoCapacity = 1, float reloadTime = 1f)
    {
        if(ammoCapacity <= 0)
        {
            //To prevent division by zero
            ammoCapacity = 1;
        }

        this.reloadingData = new PFBurstData(ammoCapacity, reloadTime);
        this.burstData = new List<PFBurstData>();
        this.burstAndReload = new List<PFBurstData>();

        burstAndReload.Insert(0, reloadingData);
    }

    public void AddBurstData(int n, float r)
    {
        this.burstData.Add(new PFBurstData(n, r));

        //Sort by N
        burstData.Sort((A, B) => { return B.n.CompareTo(A.n); });

        this.burstAndReload = burstData.Select(c => c).ToList();
        burstAndReload.Insert(0, reloadingData);
    }


    /*
     * N is the number of bullets in a burst. 
     * A burst can be a single bullet, or 
     * represent burst fire (e.g. triple shot)
     * or represent reload time. In fact, the 
     * largest N is the clip size
     * 
     * For decreasing N, we see if the ammo
     * remaining is divisible by N. If so, we return
     * the associated wait time. 
     * We run by decreasing values, since if a number
     * is divisible by x, then it will be divisible
     * by x * y, so larger N would be ignored.
     * 
     */
    /// <summary>
    /// Finds the correct wait time from the provided possible times.
    /// </summary>
    /// <param name="ammoRemaining"></param>
    /// <returns></returns>
    public float GetWaitTime(int ammoRemaining)
    {
        foreach (PFBurstData x in burstAndReload)
        {
            if ((ammoRemaining % x.n) == 0)
            {
                return x.r;
            }
        }
        return baseRate;
    }
}


[Serializable]
public class PFBurstData
{
    public int n;//umberOfShotsInBurst
    public float r;//SecondsBetweenShots

    public PFBurstData(int n, float r)
    {
        this.n = n;
        this.r = r;
    }
}