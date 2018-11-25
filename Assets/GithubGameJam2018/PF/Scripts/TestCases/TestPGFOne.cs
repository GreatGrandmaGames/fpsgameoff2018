using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[Obsolete("Test Case Outdated")]
public class TestPGFOne : MonoBehaviour {

    public static TestPGFOne Instance { get; private set; }

    public Action<ParametricFirearm> onSetCurrentPGF;

    public ParametricFirearm Current
    {
        get
        {
            return currentPGF;
        }
        private set
        {
            currentPGF = value;
            
            if(onSetCurrentPGF != null)
            {
                onSetCurrentPGF.Invoke(value);
            }
        }
    }

    public TextMeshProUGUI gunName;
    public TextMeshProUGUI ammo;
    public TextMeshProUGUI description;

    private ParametricFirearm currentPGF;

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        GenerateNewPGF();
	}


    // Update is called once per frame
	void Update () {

        //Generate new
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateNewPGF();
        };

        //If we have no pgf, return from here
        if(currentPGF == null)
        {
            return;
        }

        //Fire
        if (Input.GetMouseButtonDown(1)){
            Debug.Log("FIRE IN THE HOLE");

            currentPGF.TriggerPress();
        }

        //Reload
        if (Input.GetKeyDown(KeyCode.R))
        {   //ammo.text = "RELOADING";

            currentPGF.ManualReload();
        }
	}

    void GenerateNewPGF()
    {
        if(currentPGF != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Generated"));
        }

        foreach (GameObject x in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(x);
        };

        Current = PFFactory.Instance.CreatePF();


        gunName.text = Current.Data.Meta.name;
    }
}
