using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct EventPositions
{
    public HumanEventType HumanEventType;
    public List<Vector3> Positions;
    public Transform[] TargetTransforms;
}
