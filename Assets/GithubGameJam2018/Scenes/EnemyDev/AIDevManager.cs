using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDevManager : MonoBehaviour {

    public Damageable player;
    public Transform enemyParent;

    private void Start()
    {
        foreach(PF_AI pfai in enemyParent.GetComponentsInChildren<PF_AI>())
        {
            pfai.SetTarget(player);
        }       
    }
}
