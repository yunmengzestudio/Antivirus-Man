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
    public string[] PoliceWarningTexts;     // 被大爷抓了之后说的文字
    public AudioClip[] PoliceWarningAudios; // 被大爷抓了之后说的话
}
