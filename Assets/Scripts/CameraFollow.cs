using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour {

    public GameObject Target;
    public float SmoothTime = 0.2f;
    
    private Vector3 offset;
    private Vector3 velocity;


	void Awake () {
        offset = transform.position - Target.transform.position;
    }
	

	void FixedUpdate () {
        transform.position = Vector3.SmoothDamp(
            transform.position
            , Target.transform.position + offset
            , ref velocity, SmoothTime);
	}

}
