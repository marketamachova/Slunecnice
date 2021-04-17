using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
    public class FoveatedRendering : MonoBehaviour
    {
        void Start()
        {
            OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.High;
            OVRManager.useDynamicFixedFoveatedRendering = true;
        }
    }
}