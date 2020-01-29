using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float Step = 0.05f;
    public JoyStick joyStick;

    private Rigidbody rigidbody;

    void Awake () {
        rigidbody = GetComponent<Rigidbody>();
        //updateStep = Step * Time.deltaTime;
	}

    void FixedUpdate () {

        float h = joyStick.Horizontal();
        float v = joyStick.Vertical();
        

        if (Mathf.Abs(h) > 0.01 || Mathf.Abs(v) > 0.01) {
            rigidbody.transform.position += new Vector3(h * Step, 0, v * Step);
        }


        if (Input.GetKey(KeyCode.Space)) {
            rigidbody.velocity = Vector3.zero;
        }
	}
}
