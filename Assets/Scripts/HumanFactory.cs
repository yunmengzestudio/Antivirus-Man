using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class HumanFactory : MonoBehaviour
{
    public float Interval = 6f;
    public float AccelerateRatio = 0.05f;   // 每次加快的比例
    public float MinInterval = 1f;

    public string HumanPrefab = "Human";
    public Vector3[] BornPositions;
    public HumanEvent[] HumanEvents;

    private ResLoader resLoader;
    private float timer;


    private void Awake() {
        resLoader = ResLoader.Allocate();
        timer = Interval;
    }

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            Interval -= Interval * AccelerateRatio;
            Interval = Mathf.Max(Interval, MinInterval);
            timer = Interval;
            GenerateOne();
        }
    }

    private void GenerateOne() {
        // 随机生成初始坐标、模型
        Vector3 bornPos = BornPositions[Random.Range(0, BornPositions.Length)];
        HumanEvent humanEvent = HumanEvents[Random.Range(0, HumanEvents.Length)];
        string prefab = humanEvent.Prefabs[Random.Range(0, humanEvent.Prefabs.Length)];

        // 实例化
        GameObject go = resLoader.LoadSync<GameObject>(HumanPrefab).Instantiate();
        go.transform.parent = transform;
        go.transform.position = bornPos;
        // 人物模型加载
        GameObject model = resLoader.LoadSync<GameObject>(prefab).Instantiate();
        model.transform.parent = go.transform;
        model.transform.localPosition = Vector3.zero;

        // 初始化 Human
        Human human = go.GetComponent<Human>();
        human.Init(humanEvent);
    }
}
