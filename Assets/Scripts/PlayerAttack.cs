using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode AttackKeyCode = KeyCode.J;
    public TouchButton AttackBtn;
    public Animator Animator;
    public GameObject DamageCollider;

    private float attackDuration;
    private float timer = 0f;


    private void Start() {
        if (AttackBtn) {
            AttackBtn.PointerDownEvent += (s, e) => Attack();
        }
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
}
