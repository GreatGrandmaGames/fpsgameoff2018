using System;
using UnityEngine;

[Serializable]
public class PFTrajectoryData
{
    public float initialSpeed;
    public float maxInitialSpreadAngle; //measured in angle per meter per second
    public float dropOffRatio; // a ratio
}
