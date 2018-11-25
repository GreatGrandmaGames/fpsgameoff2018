using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagrangeTest1 : MonoBehaviour {

    private void Start()
    {
        var int1DP = new Dictionary<float, float>()
        {
            { 1, 1 },
            { 2, 8 },
            { 3, 27 },
        };

        var int1Test = new Dictionary<float, float>()
        {
            { 8, 302 },
            { -5, 211 },
            { 0, 6 },
        };

        Debug.Log("Interpolating");

        var interpolation1 = Calculus.LagrangeInterpolate(int1DP);

        foreach (var kv in int1DP)
        {
            Debug.Log("Tesing supplied data points. Input is " + kv.Key + ", output should be " + kv.Value + " and interpolation says " + interpolation1(kv.Key));
        }

        foreach (var kv in int1Test)
        {
            Debug.Log("Tesing supplied data points. Input is " + kv.Key + ", output should be " + kv.Value + " and interpolation says " + interpolation1(kv.Key));
        }
    }
}
