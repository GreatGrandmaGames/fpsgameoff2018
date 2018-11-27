using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class PF_AITest : MonoBehaviour {

    public PF_AI testee;

    private void Awake()
    {
        testee.Data = new PF_AIData()
        {
            moveSpeed = 3.5f,
            firingMoveSpeedFactor = 0.5f,
        };
    }

    private void Start()
    {
        testee.SetTarget(GetComponent<Damageable>());
    }
}
