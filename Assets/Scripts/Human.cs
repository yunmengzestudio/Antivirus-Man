using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Human : MonoBehaviour
{
    public NavMeshAgent Agent;


    public void Init(HumanEvent humanEvent) {
        // Get target position
        // GameManager.Instance.LevelManager.GetTargetPos(humanEvent.EventType);

        // Move to target pos
        // Agent.Warp()
    }

    // Got TOUCH by POLICE -> GO Home
}
