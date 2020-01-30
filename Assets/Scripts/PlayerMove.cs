using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class PlayerMove : MonoBehaviour
{
    public float Step = 0.5f;
    public float RotateSpeed = 10f;
    [Inject]
    public JoyStick mJoyStick { get; set; }
    public Animator Animator;

    private new Rigidbody rigidbody;


    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        App.Container.Inject(this);
    }

    void FixedUpdate() {
        float h = mJoyStick.Horizontal();
        float v = mJoyStick.Vertical();
        float speed = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));

        if (speed > 0.01) {
            rigidbody.transform.position += new Vector3(h * Step, 0, v * Step);
            Rotating(h, v);
        }
        Animator.SetFloat("Speed", speed > 0.01 ? speed * 1.5f : speed);
    }

    void Rotating(float h, float v) {
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(h, 0f, v), Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, 10 * Time.deltaTime);

        rigidbody.MoveRotation(newRotation);
    }
}
