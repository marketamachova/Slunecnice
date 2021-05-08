using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayTransformSetter : MonoBehaviour
{
    [SerializeField] private OVRHand hand;
    [SerializeField] private GameObject handAnchor;
    [SerializeField] private OVRInputModule ovrInputModule;
    
    private void Start()
    {
        SetRayTransform();
    }

    void Update()
    {
        SetRayTransform();
    }

    void SetRayTransform()
    {
        Transform t = hand.PointerPose;
        ovrInputModule.rayTransform = t;
    }
}
