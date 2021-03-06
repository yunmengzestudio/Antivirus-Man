﻿using System.Collections;
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
    private bool dead = false;

    private NavMeshObstacle Obstacle;


    private void Update() {
        animator.SetFloat("Speed", Agent.velocity.magnitude / SpeedRatio);
    }

    private void OnTriggerStay(Collider other) {
        OnTriggerEnter(other);
    }

    private void OnTriggerEnter(Collider other) {
        if (!dead && other.tag == "Damage") {
            dead = true;
            StopCoroutine(mission);
            StartCoroutine(GOHome());
            
            // 告诉 Manager 该项活动被阻止，进行记录
            ReportMission(done: false);
        }
    }


    public void Init(HumanEvent humanEvent) {
        // 初始化引用
        this.humanEvent = humanEvent;
        animator = GetComponentInChildren<Animator>();
        BornPos = transform.position;   // 记录出生点位置（HOME）
        FluentText.InitPanel(transform, FluentText.transform.position - transform.position);
        Obstacle = GetComponent<NavMeshObstacle>();
        SwitchAgentObstacle(true);

        // 开始执行任务
        mission = StartCoroutine(GO());
    }


    private IEnumerator GO() {
        yield return new WaitForSeconds(1f);
        FluentText.ChangeWord(humanEvent.TipTexts[Random.Range(0, humanEvent.TipTexts.Length)]);
        FluentText.ShowPanel();

        // Get target position and Move to target pos
        missionPos = App.Container.Resolve<LevelManager>().GetTargetPos(humanEvent.EventType);
        
        SwitchAgentObstacle(true);
        Agent.SetDestination(RandomNearPos(missionPos, Agent.stoppingDistance / 2));

        while (!ArriveTarget()) {
            yield return null;  // 等待到下一帧
        }


        SwitchAgentObstacle(false);

        // 到达指定地点后进行结算
        if (humanEvent.IsParty) {
            var lookDir = missionPos - transform.position;
            lookDir.Set(lookDir.x, 0, lookDir.z);
            Quaternion q = Quaternion.LookRotation(lookDir);
            transform.rotation = q;
            // 播放动画
            string anim = humanEvent.PartyAnimations[
                Random.Range(0, humanEvent.PartyAnimations.Length)];
            animator.PlayInFixedTime(anim,-1,1f);

            // 告诉 Manager 自己到达某处聚集点
            TypeEventSystem.Send(new PartyNotification(missionPos, transform));
            yield return new WaitForSeconds(2);
            FluentText.HidePanel();
        }
        else {
            ReportMission(done: true);
            Destroy(gameObject);
        }
    }
    

    private IEnumerator GOHome() {
        // 恢复 animator 到初始状态
        animator.Play("Idle");

        // 被抓之后说的话
        string words = humanEvent.WordsAfterCaught[Random.Range(0, humanEvent.WordsAfterCaught.Length)];
        FluentText.ChangeWord(words);

        SwitchAgentObstacle(true);
        Agent.SetDestination(BornPos);
        Agent.speed = 3.5f;
        SpeedRatio = 1;
        while (!ArriveTarget()) {
            yield return null;
        }
        // Do something ...
        Destroy(gameObject, 1f);
    }

    // 告诉 Manager 该项活动是否被阻止，进行记录
    private void ReportMission(bool done) {
        // 活动被阻止
        if (!done) {
            if (humanEvent.IsParty) {
                TypeEventSystem.Send(new PartyNotification(missionPos, transform, true));
            }
            TypeEventSystem.Send(new MissionExpelledNotification(humanEvent));
        }
        // 活动成功完成
        else {
            TypeEventSystem.Send(new MissionCompletedNotification(humanEvent));
        }

        Debug.Log("Mission: " + humanEvent.EventType.ToString() + (done ? "完成" : "被阻止"));
    }

    // Agent 是否到达当前目的地
    private bool ArriveTarget() {
        return (Agent.destination - Agent.nextPosition).magnitude
            <= Agent.stoppingDistance + 0.05f;
    }

    private void SwitchAgentObstacle(bool enableAgent) {
        Agent.enabled = enableAgent;
        Obstacle.enabled = !enableAgent;
    }

    private Vector3 RandomNearPos(Vector3 pos, float radius) {
        Vector2 p = Random.insideUnitCircle.normalized * radius;
        return new Vector3(pos.x + p.x, 0, pos.z + p.y);
    }
}
