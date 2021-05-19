using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /**
     * sets ray transform origin to accurate position within virtual hands
     */
    public class RayTransformSetter : MonoBehaviour
    {
        [SerializeField] private OVRHand hand;
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
}
