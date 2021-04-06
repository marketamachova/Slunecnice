using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoveatiedRendering : MonoBehaviour
{
    void Start()
    {
        OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.High;
        OVRManager.useDynamicFixedFoveatedRendering = true;
    }

}
