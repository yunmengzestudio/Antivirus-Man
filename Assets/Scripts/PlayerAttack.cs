using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode AttackKeyCode = KeyCode.J;
    public TouchButton AttackBtn;
    public Animator Animator;


    private void Start() {
        if (AttackBtn) {
            AttackBtn.PointerDownEvent += (s, e) => Attack();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(AttackKeyCode)) {
            Attack();
        }
    }

    private void Attack() {
        Animator.SetTrigger("Attack");
        // Attack 事件处理
    }
}
