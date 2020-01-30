using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public EventPositions[] Positions;

    private Dictionary<HumanEventType, EventPositions> Event2Pos;


    private void Awake() {
        App.Container.RegisterInstance<LevelManager>(this);

        ResMgr.Init();
        InitEvent2Pos();
    }

    /// <summary>
    /// 获取各类活动的目的地点
    /// </summary>
    public Vector3 GetTargetPos(HumanEventType type) {
        if (!Event2Pos.ContainsKey(type))
            return Vector3.zero;

        EventPositions eventPositions = Event2Pos[type];
        if (eventPositions.Positions.Length > 0)
            return eventPositions.Positions[Random.Range(0, eventPositions.Positions.Length)];
        else
            return Vector3.zero;
    }


    // 构造字典 Event2Pos
    private void InitEvent2Pos() {
        Event2Pos?.Clear();
        Event2Pos = new Dictionary<HumanEventType, EventPositions>();

        foreach (var pos in Positions) {
            if (Event2Pos.ContainsKey(pos.HumanEventType)) {
                string log = string.Format("LevelManager 中[%s]活动地点重复", pos.HumanEventType.ToString());
                Debug.LogWarning(log);
                continue;
            }
            Event2Pos.Add(pos.HumanEventType, pos);
        }
    }
}
