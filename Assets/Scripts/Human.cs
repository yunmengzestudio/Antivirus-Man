using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using QFramework;


public class Human : MonoBehaviour
{
    public FluentTextController FluentText;
    public NavMeshAgent Agent;
    public float SpeedRatio = 1f;   // NavMeshAgent 的速度转 Animator Speed 的系数

    private HumanEvent humanEvent;
    private Animator animator;

    private Vector3 BornPos;
    private Coroutine mission;
    private Vector3 missionPos;


    private void Update() {
        animator.SetFloat("Speed", Agent.velocity.magnitude / SpeedRatio);
    }

    private void OnTriggerStay(Collider other) {
        OnTriggerEnter(other);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Damage") {
            StopCoroutine(mission);
            StartCoroutine(GOHome());

            ReportMission(done: false);// 告诉 Manager 该项活动被阻止，进行记录
            if (humanEvent.EventType == HumanEventType.Party) {
                TypeEventSystem.Send(new PartyNotification(missionPos, transform, true));
            }
        }
    }


    public void Init(HumanEvent humanEvent) {
        // 初始化引用
        this.humanEvent = humanEvent;
        animator = GetComponentInChildren<Animator>();
        BornPos = transform.position;   // 记录出生点位置（HOME）
        FluentText.InitPanel(transform, FluentText.transform.position - transform.position);

        // 开始执行任务
        mission = StartCoroutine(GO());
    }


    private IEnumerator GO() {
        yield return new WaitForSeconds(1f);
        FluentText.ChangeWord(humanEvent.TipTexts[Random.Range(0, humanEvent.TipTexts.Length)]);
        FluentText.ShowPanel();

        // Get target position and Move to target pos
        missionPos = App.Container.Resolve<LevelManager>().GetTargetPos(humanEvent.EventType);
        Agent.SetDestination(missionPos);

        while (!ArriveTarget()) {
            yield return null;  // 等待到下一帧
        }
        
        // 到达指定地点后进行结算
        if (humanEvent.EventType == HumanEventType.Party) {
            Debug.Log("到咯!");
            FluentText.ChangeWord("到咯！");
            // 告诉 Manager 自己到达某处聚集点
            TypeEventSystem.Send(new PartyNotification(missionPos, transform));
        }
        else {
            ReportMission(done: true);
            Destroy(gameObject);
        }
    }
    

    private IEnumerator GOHome() {
        FluentText.ChangeWord("怕了怕了！我回家了！");
        Agent.SetDestination(BornPos);
        while (!ArriveTarget()) {
            yield return null;
        }
        // Do something ...
        Destroy(gameObject, 1f);
    }

    // 告诉 Manager 该项活动是否被阻止，进行记录
    private void ReportMission(bool done) {
        Debug.Log("Mission: " + humanEvent.EventType.ToString() + (done ? "完成" : "被阻止"));
    }

    // Agent 是否到达当前目的地
    private bool ArriveTarget() {
        return (Agent.destination - Agent.nextPosition).magnitude
            <= Agent.stoppingDistance + 0.05f;
    }
}
