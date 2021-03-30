using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteraction : MonoBehaviour
{
    [SerializeField] private OVRInputModule ovrInputModule;
    [SerializeField] private OVRRaycaster ovrRaycaster;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject rayCasterAnchor;
    private OVRHand _hand;
    private void Start()
    {
        // _hand = hand.GetComponent<OVRHand>();
        // // var indexTransform = hand.GetComponent<OVRSkeleton>().Bones[20].Transform;
        //
        // ovrInputModule.rayTransform = _hand.PointerPose;
        // ovrInputModule.rayTransform.position = _hand.PointerPose.position + new Vector3(0, 0.2f, 0);
        // ovrInputModule.rayTransform.rotation = Quaternion.Euler(0, 90, 0);
        //
        // // VRDebugger.Instance.Log("index transform " + indexTransform);
        // VRDebugger.Instance.Log("ovrInputModule.rayTransform " + ovrInputModule.rayTransform.position);
        // VRDebugger.Instance.Log("ovrInputModule.rayTransform rotation" + ovrInputModule.rayTransform.rotation);
        // VRDebugger.Instance.Log("rayCasterTransform pos" + rayCasterAnchor.transform.position);
        //
        // Debug.Log(ovrInputModule.rayTransform.rotation);
        // Debug.Log(ovrInputModule.rayTransform.position);
        // Debug.Log(_hand.PointerPose.position);
        // Debug.Log("rayCasterTransform pos" + rayCasterAnchor.transform.position);
        //
        //
        // // ovrInputModule.rayTransform = _hand.PointerPose;
        // // ovrRaycaster.pointer = _hand.PointerPose.gameObject;
        
    }

    // void Update()
    // {
    //     VRDebugger.Instance.Log("rayCasterTransform pos" + rayCasterAnchor.transform.position);
    // }

}
