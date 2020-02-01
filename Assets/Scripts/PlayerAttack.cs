using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;


public class PlayerAttack : MonoBehaviour
{
    public KeyCode AttackKeyCode = KeyCode.J;
    public TouchButton AttackBtn;
    public Animator Animator;
    public GameObject DamageCollider;
    public FluentTextController FluentText;
    public AudioSource Audio;

    private float attackDuration;
    private float timer = 0f;
    private ResLoader resLoader;


    private void Start() {
        if (AttackBtn) {
            AttackBtn.PointerDownEvent += (s, e) => Attack();
        }

        // FluentText 初始化
        FluentText.InitPanel(transform, FluentText.transform.position - transform.position);
        FluentText.ShowPanel();

        // 注册驱逐通知
        TypeEventSystem.Register<MissionExpelledNotification>(OnExpelOne);

        // ResLoader Allocate
        resLoader = ResLoader.Allocate();

        // 获取攻击动画时长
        if (Animator) {
            AnimationClip[] clips = Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips) {
                if (clip.name == "Attack") {
                    attackDuration = clip.length;
                    break;
                }
            }
        }
    }

    private void OnDestroy() {
        TypeEventSystem.UnRegister<MissionExpelledNotification>(OnExpelOne);
    }

    private void Update() {
        if (Input.GetKeyDown(AttackKeyCode)) {
            Attack();
        }
        
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else if (timer < 0) {
            timer = 0;
            // 关闭碰撞器
            DamageCollider.tag = "Player";
        }
    }

    private void Attack() {
        Animator.SetTrigger("Attack");
        // Attack 事件处理
        timer = attackDuration;
        // 开启碰撞器
        DamageCollider.tag = "Damage";
    }

    // 成功驱逐村民的处理
    private void OnExpelOne(MissionExpelledNotification humanEvent) {
        // Text
        int i = Random.Range(0, humanEvent.PoliceWarningTexts.Length);
        FluentText.ChangeWord(humanEvent.PoliceWarningTexts[i]);
        
        // Audio
        int index = Random.Range(0, humanEvent.PoliceWarningAudios.Length);
        Audio.clip = resLoader.LoadSync<AudioClip>(humanEvent.PoliceWarningAudios[index]);
        Audio.Play();
    }

}
