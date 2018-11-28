using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TmpCooldownInfo : MonoBehaviour {

    private TextMeshProUGUI tmpro;
    // Use this for initialization
    void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ZeroG zerog = GameObject.FindGameObjectWithTag("Player").GetComponent<ZeroG>();
        int num = (int)zerog.currentFuelTime;
        tmpro.text = num.ToString();//CurrentAmmo.ToString();
    }
}
