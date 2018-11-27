using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunInfo : MonoBehaviour {

    private TextMeshProUGUI tmpro;
	// Use this for initialization
	void Start () {
        tmpro = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        tmpro.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPFController>().pf.CurrentAmmo.ToString();//CurrentAmmo.ToString();
    }
}
