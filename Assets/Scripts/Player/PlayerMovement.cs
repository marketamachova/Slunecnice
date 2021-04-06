using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private List<PathCreator> _pathCreators;
        private PathCreator _currentPathCreator;
        private GameController _controller;
        public Vector3 offset = new Vector3(0, 5, 0);
        public float speed;
        public bool rotateCamera;
        private float _distance;
        private bool _shiftCamera = false;
        private Animator _animator;
        private int _counter = 0;

        private static readonly int Drive = Animator.StringToHash("Drive");


        void Start()
        {
            _controller = FindObjectOfType<GameController>();

            _currentPathCreator = _pathCreators[0];
            transform.position = _currentPathCreator.path.GetPoint(0) + offset;
            _animator = GetComponentInChildren<Animator>();
            if (_animator)
            {
                _animator.SetTrigger(Drive);
            }
        }

        void Update()
        {
            var cameraOffset = 0f;
            if (_shiftCamera) cameraOffset += 0.1f;

            _distance += speed * Time.deltaTime;
            transform.position = _currentPathCreator.path.GetPointAtDistance(_distance + cameraOffset) + offset;
            if (rotateCamera)
            {
                transform.rotation = _currentPathCreator.path.GetRotationAtDistance(_distance);
            }

            if (_distance >= _currentPathCreator.path.length)
            {
                if (_currentPathCreator == _pathCreators[_pathCreators.Count - 1])
                {
                    _controller.End();
                }

                _counter++;
                _currentPathCreator = _pathCreators[_counter];
                _distance = 0;
                transform.position = _currentPathCreator.path.GetPoint(0) + offset;
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

        public void SetPathCreator(List<PathCreator> pathCreators)
        {
            Debug.Log("setting path creators");
            _pathCreators = pathCreators;
        }
    }
}