using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public EventPositions[] Positions;
    public Text MissionConditionText;

    private ResLoader resLoader;
    private Dictionary<HumanEventType, EventPositions> Event2Pos;
    private Dictionary<Vector3, List<Transform>> PartyPos2Human;
    private List<FluentTextController> partyTipTexts;

    private List<int> missionExpelledRecord;      // 被驱逐的事件记录
    private List<int> missionCompletedRecord;     // 事件完成的记录


    private void Awake() {
        // 注册 IOC
        App.Container.RegisterInstance<LevelManager>(this);
        // 注册聚会通知的提醒
        TypeEventSystem.Register<PartyNotification>(OnPartyNotify);
        // 注册活动成功/失败的通知提醒
        TypeEventSystem.Register<MissionCompletedNotification>(OnMissionCompleteNotify);
        TypeEventSystem.Register<MissionExpelledNotification>(OnMissionExpelledNotify);

        ResMgr.Init();
        resLoader = ResLoader.Allocate();

        InitTargetPositions();
        InitEvent2Pos();
        InitPartyPos2Human();
        InitPartyTipTexts();
        InitRecordList();
    }

    private void OnDestroy() {
        TypeEventSystem.UnRegister<PartyNotification>(OnPartyNotify);
        TypeEventSystem.UnRegister<MissionCompletedNotification>(OnMissionCompleteNotify);
        TypeEventSystem.UnRegister<MissionExpelledNotification>(OnMissionExpelledNotify);
    }

    private void FixedUpdate() {
        if (partyTipTexts.Count > 0) {
            CheckParty();
        }

        if (MissionConditionText) {
            RefreshMissionConditionText();
        }
    }


    /// <summary>
    /// 获取各类活动的目的地点
    /// </summary>
    public Vector3 GetTargetPos(HumanEventType type) {
        if (!Event2Pos.ContainsKey(type))
            return Vector3.zero;

        EventPositions eventPositions = Event2Pos[type];
        if (eventPositions.Positions.Count > 0)
            return eventPositions.Positions[Random.Range(0, eventPositions.Positions.Count)];
        else
            return Vector3.zero;
    }


    #region 活动完成/失败的消息处理

    private void OnMissionCompleteNotify(MissionCompletedNotification notif) {
        ++missionCompletedRecord[(int)notif.EventType];
    }

    private void OnMissionExpelledNotify(MissionExpelledNotification notif) {
        ++missionExpelledRecord[(int)notif.EventType];
    }

    private void RefreshMissionConditionText() {
        string[] labels = System.Enum.GetNames(typeof(HumanEventType));

        string s = "[已完成事件]\n";
        for(int i = 0; i < labels.Length; i++) {
            s = s + labels[i] + ":" + missionCompletedRecord[i] + "\n";
        }
        s = s + "[已阻止事件]\n";
        for (int i = 0; i < labels.Length; i++) {
            string label = ((HumanEventType)i).ToString();
            s = s + labels[i] + ":" + missionExpelledRecord[i] + "\n";
        }
        MissionConditionText.text = s;
    }

    #endregion


    #region 聚集点管理 Party

    // 处理聚集通知，在聚集人员 List 添加 or 删除
    private void OnPartyNotify(PartyNotification notification) {
        if (!PartyPos2Human.ContainsKey(notification.PartyPosition))
            return;

        List<Transform> humen = PartyPos2Human[notification.PartyPosition];
        if (notification.Leaving) {
            humen.Remove(notification.HumanTransform);
        }
        else {
            humen.Add(notification.HumanTransform);
        }
    }

    // 聚集情况检查，并给予提示
    private void CheckParty() {
        if (!Event2Pos.ContainsKey(HumanEventType.Party)) {
            return;
        }
        Vector3[] pos = Event2Pos[HumanEventType.Party].Positions.ToArray();
        for (int i = 0; i < pos.Length; i++) {
            List<Transform> humen = PartyPos2Human[pos[i]];
            if (partyTipTexts[i].CurrentText != humen.Count.ToString())
                partyTipTexts[i].ChangeWord(humen.Count.ToString());
        }
    }

    #endregion


    #region 初始化


    public void InitTargetPositions()
    {
        for(int i=0;i<Positions.Length;i++)
        {
            Positions[i].Positions = new List<Vector3>();
            foreach(var point in Positions[i].TargetTransforms)
            {
                Positions[i].Positions.Add(point.position);
            }
        }
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

    // 构造字典 PartyPos2Human
    private void InitPartyPos2Human() {
        PartyPos2Human?.Clear();
        PartyPos2Human = new Dictionary<Vector3, List<Transform>>();

        if (!Event2Pos.ContainsKey(HumanEventType.Party))
            return;

        EventPositions positions = Event2Pos[HumanEventType.Party];
        foreach (Vector3 pos in positions.Positions) {
            PartyPos2Human.Add(pos, new List<Transform>());
        }
    }

    // 构造聚会提示文字
    private void InitPartyTipTexts() {
        partyTipTexts?.Clear();
        partyTipTexts = new List<FluentTextController>();

        if (!Event2Pos.ContainsKey(HumanEventType.Party))
            return;

        GameObject empty = new GameObject("PartyTexts");
        empty.transform.parent = transform;
        empty.transform.localPosition = Vector3.zero;

        foreach (Vector3 pos in Event2Pos[HumanEventType.Party].Positions) {
            GameObject go = resLoader.LoadSync<GameObject>("FluentTextCanvas").Instantiate();
            go.transform.parent = empty.transform;
            go.GetComponentsInChildren<Text>().ForEach(Text => {
                Text.fontSize = 120;
                Text.color = Color.red;
            });

            FluentTextController fluentText = go.GetComponent<FluentTextController>();
            fluentText.InitPanel(empty.transform, Vector3.zero);
            fluentText.ShowPanel();
            fluentText.transform.localPosition = new Vector3(pos.x, 5, pos.z);
            partyTipTexts.Add(fluentText);
        }
    }

    // 构造记录列表
    private void InitRecordList() {
        int len = System.Enum.GetNames(typeof(HumanEventType)).Length;
        missionExpelledRecord = new List<int>(len);
        missionCompletedRecord = new List<int>(len);
        for (int i = 0; i < len; i++) {
            missionExpelledRecord.Add(0);
            missionCompletedRecord.Add(0);
        }
    }

    #endregion
}
