using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PartyNotification
{
    public Vector3 PartyPosition;
    public Transform HumanTransform;
    public bool Leaving;

    public PartyNotification(Vector3 partyPos, Transform humanTransform, bool leaving = false) {
        PartyPosition = partyPos;
        HumanTransform = humanTransform;
        Leaving = leaving;
    }
}
