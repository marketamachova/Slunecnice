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
        VRDebugger.Instance.Log("setting ovr input module ray transform");
        SetRayTransform();
        VRDebugger.Instance.Log("rayTransform pos " + ovrInputModule.rayTransform.position);
        VRDebugger.Instance.Log("rayTransform rot " + ovrInputModule.rayTransform.rotation);
    }

    void Update()
    {
        SetRayTransform();
    }

    void SetRayTransform()
    {
        Transform t = hand.PointerPose;
        // t.position += new Vector3(0, 1.6f, -0.5f);
        
        ovrInputModule.rayTransform = t;
    }
}
