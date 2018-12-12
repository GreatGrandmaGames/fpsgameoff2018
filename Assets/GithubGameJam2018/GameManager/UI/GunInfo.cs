using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunInfo : MonoBehaviour {

    public PlayerPFController playerPF;

    public TextMeshProUGUI gunName;
    public TextMeshProUGUI ammoCount;

    private ParametricFirearm current;

    private void Start()
    {
        playerPF.OnSwitchWeapon += SetUpGunUI;

        SetUpGunUI(null, playerPF.Current);
    }

    private void SetUpGunUI(ParametricFirearm old, ParametricFirearm curr)
    {
        if (old != null)
        {
            old.OnFire -= SetAmmoText;
            old.OnReload -= SetAmmoText;
        }

        if (curr != null)
        {
            current = curr;

            gunName.text = playerPF.Current.Data.Meta.name;

            curr.OnFire += SetAmmoText;
            curr.OnReload += SetAmmoText;
        }
    }

    private void SetAmmoText()
    {
        if(current != null)
        {
            ammoCount.text = current.CurrentAmmo + "/" + current.Data.RateOfFire.AmmoCapacity;
        }
    }
}
