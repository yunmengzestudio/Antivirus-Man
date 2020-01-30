using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<List<Vector2>> Positions = new List<List<Vector2>>();


    private void Awake() {
        // GameManager.Instance.LevelManager = this;
    }

    /// <summary>
    /// 获取各类活动的目的地点
    /// </summary>
    public Vector2 GetTargetPos(HumanEvent humanEvent) {
        List<Vector2> posList = Positions[(int)humanEvent.EventType];
        if (posList.Count > 0)
            return posList[Random.Range(0, posList.Count)];
        else
            return Vector2.zero;
    }
}
