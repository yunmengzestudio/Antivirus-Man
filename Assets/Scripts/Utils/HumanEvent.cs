using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HumanEventType
{
    Drinking,       // 去喝酒
    KTV,            // 去KTV
    Shopping,       // 去逛街
    InternetBar,    // 去网吧
    Party           // 聚集
}

[System.Serializable]
public class HumanEvent
{
    public HumanEventType EventType;
    public string[] Prefabs;                // 人物 Prefabs
    public string[] TipTexts;               // 出门后头顶的提示文字
    public string[] PartyAnimations;        // 到达聚堆点后播放的动画

    public string[] PoliceWarningTexts;     // 被大爷抓了之后说的文字
    public AudioClip[] PoliceWarningAudios; // 被大爷抓了之后说的话

    public HumanEvent() { }

    public HumanEvent(HumanEvent other) {
        EventType = other.EventType;
        Prefabs = other.Prefabs;
        TipTexts = other.TipTexts;
        PoliceWarningTexts = other.PoliceWarningTexts;
        PoliceWarningAudios = other.PoliceWarningAudios;
    }
}

public class MissionExpelledNotification : HumanEvent
{
    public MissionExpelledNotification(HumanEvent e) : base(e) { }
}

public class MissionCompletedNotification : HumanEvent
{
    public MissionCompletedNotification(HumanEvent e) : base(e) { }
}