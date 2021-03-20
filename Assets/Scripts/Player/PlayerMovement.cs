using PathCreation;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public PathCreator pathCreator;
        public Vector3 offset = new Vector3(0, 5, 0);
        public float speed;
        public bool rotateCamera;
        private float _distance;
        private bool _shiftCamera = false;

        void Start()
        {
            transform.position = pathCreator.path.GetPoint(0) + offset;
        }
    
        void Update()
        {
            var cameraOffset = 0f;
            if (_shiftCamera) cameraOffset += 0.1f;
            
            _distance += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(_distance + cameraOffset) + offset;
            if (rotateCamera)
            {
                transform.rotation = pathCreator.path.GetRotationAtDistance(_distance);
            }
        }

        public void ShiftCamera()
        {
            _shiftCamera = true;
        }
        
        public void UnShiftCamera()
        {
            _shiftCamera = false;
        }
    }
}
