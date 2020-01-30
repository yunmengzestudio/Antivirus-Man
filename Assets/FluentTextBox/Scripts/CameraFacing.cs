﻿using UnityEngine;
using System.Collections;

public class CameraFacing : MonoBehaviour
{   //挂在需要看向摄像机的UI物体上（例如血条，伤害冒字）

    private Camera refCamera;
    public bool reverFace = false;
    private Transform mRoot;

    private void Awake()
    {
        if (!refCamera)
        {
            refCamera = Camera.main;
        }
        mRoot = transform;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = mRoot.position + refCamera.transform.rotation * (reverFace ? Vector3.back : Vector3.forward);
        Vector3 targetOrientation = refCamera.transform.rotation * Vector3.up;
        mRoot.LookAt(targetPos, targetOrientation);
    }
}