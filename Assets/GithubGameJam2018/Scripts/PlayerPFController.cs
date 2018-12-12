using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPFController : MonoBehaviour {

    //Notable Events
    public Action<ParametricFirearm, ParametricFirearm> OnSwitchWeapon;
    //end

    public KeyCode scrollWeaponsKey = KeyCode.F;
    public KeyCode reload = KeyCode.R;

    [SerializeField]
    public List<ParametricFirearm> inventory;

    private ParametricFirearm current;
    public ParametricFirearm Current
    {
        get
        {
            return current;
        }
        private set
        {
            var old = current;
            if (old != null)
            {
                old.gameObject.SetActive(false);
            }

            current = value;

            if (current != null)
            {
                current.gameObject.SetActive(true);
            }

            if (OnSwitchWeapon != null)
            {
                OnSwitchWeapon(old, current);
            }
        }
    }
    private int currentIndex;

    private void Awake()
    {
        if(inventory == null || inventory.Count <= 0)
        {
            Debug.LogWarning("PlayerPFController: Please provide at least one PF in inspector");
        } else
        {
            Current = inventory[0];
            currentIndex = 0;
        }
    }

    public void ScrollToNextWeapon()
    {
        if(inventory == null)
        {
            return;
        }


        currentIndex = (currentIndex + 1) % inventory.Count;
        Current = inventory[currentIndex];
    }

    void Update ()
    {
        if (Input.GetKeyDown(scrollWeaponsKey))
        {
            ScrollToNextWeapon();
        }

        if(Current == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Current.TriggerPress();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Current.TriggerRelease();
        }

        if (Input.GetKeyDown(reload))
        {
            if (Current.State == ParametricFirearm.PFState.ManualReload)
            {
                Current.CancelManualReload();
            }
            else
            {
                Current.ManualReload();
            }
        }
    }
}
