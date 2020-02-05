using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;


[System.Serializable]
public struct SpeedConf
{
    public float Interval;  // 多少秒之后调整速度至 Speed
    public float Speed;     // n 秒一个人
}


public class HumanFactory : MonoBehaviour
{
    public float Interval = 6f;             // 每隔 Interval 秒生成一个
    public float AccelerateRatio = 0.05f;   // 每次加快的比例
    public float MinInterval = 1f;
    public SpeedConf[] SpeedConfs;

    public string HumanPrefab = "Human";
    public Transform[] BornPositions;
    public HumanEvent[] HumanEvents;

    private ResLoader resLoader;
    private float timer;
    private Transform humenRoot;


    private void Awake() {
        resLoader = ResLoader.Allocate();
        timer = Interval;
        InitHumenRoot();

        if (SpeedConfs?.Length > 0) {
            StartCoroutine(ConfigSpeed());
        }
    }

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            if (SpeedConfs?.Length <= 0) {
                Interval -= Interval * AccelerateRatio;
                Interval = Mathf.Max(Interval, MinInterval);
            }
            timer = Interval;
            GenerateOne();
        }
    }

    private IEnumerator ConfigSpeed(int confIndex = 0) {
        if (confIndex >= SpeedConfs.Length)
            yield break;

        yield return new WaitForSeconds(SpeedConfs[confIndex].Interval);
        Interval = SpeedConfs[confIndex].Speed;

        yield return ConfigSpeed(++confIndex);
    }

    private void GenerateOne() {
        // 随机生成初始坐标、模型
        Vector3 bornPos = BornPositions[Random.Range(0, BornPositions.Length)].position;
        HumanEvent humanEvent = HumanEvents[Random.Range(0, HumanEvents.Length)];
        string prefab = humanEvent.Prefabs[Random.Range(0, humanEvent.Prefabs.Length)];

        // 实例化
        GameObject go = resLoader.LoadSync<GameObject>(HumanPrefab).Instantiate();
        go.transform.parent = humenRoot;
        go.transform.position = bornPos;
        // 人物模型加载
        GameObject model = resLoader.LoadSync<GameObject>(prefab).Instantiate();
        model.transform.SetParent(go.transform);
        model.transform.localPosition = Vector3.zero;

        // 初始化 Human
        Human human = go.GetComponent<Human>();
        human.Init(humanEvent);
    }

    private void InitHumenRoot() {
        humenRoot = new GameObject("HumenRoot").transform;
        humenRoot.parent = transform;
        humenRoot.localPosition = Vector3.zero;
    }
}
