using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPFController : MonoBehaviour {

    public ParametricFirearm pf;
    public KeyCode reload;

    // Update is called once per frame
    void Update ()
    {
        if(pf == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            pf.TriggerPress();
        }

        if (Input.GetMouseButtonUp(0))
        {
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
    }
}
