using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private EndOfPathInstruction endOfPathInstruction;

        private PathCreator _pathCreator;
        private GameController _controller;
        [FormerlySerializedAs("_player")] [SerializeField] private GameObject player;
        
        public Vector3 offset = new Vector3(0, 5, 0);
        public float speed = 2f;
        public bool rotateCamera = true;

        private float _distance;
        private bool _shiftCamera = false;
        private Animator _animator;
        private float _time;

        private static readonly int Drive = Animator.StringToHash("Drive");

        void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("NetworkCamera");
            }
                
        }

        void Start()
        {
            _controller = FindObjectOfType<GameController>();
            _pathCreator = GameObject.FindWithTag("PathCreator").GetComponent<PathCreator>();
            
            player.transform.position = _pathCreator.path.GetPoint(0) + offset;
            _animator = player.GetComponentInChildren<Animator>();
            if (_animator)
            {
                _animator.Play(Drive);
            }
        }

        void Update()
        {
            var cameraOffset = 0f;
            if (_shiftCamera) cameraOffset += 0.1f;
            
            _time += Time.deltaTime;
            
            _distance += speed * Time.deltaTime;
            player.transform.position = _pathCreator.path.GetPointAtDistance(_distance + cameraOffset, endOfPathInstruction) + offset;
            if (rotateCamera)
            {
                player.transform.rotation = _pathCreator.path.GetRotationAtDistance(_distance, endOfPathInstruction);
            }

            if (_distance >= _pathCreator.path.length)
            {
                // if (_currentPathCreator == _pathCreators[_pathCreators.Count - 1])
                {
                    Debug.Log("calling controller.End");
                    Debug.Log("controller" + _controller);
                    _controller.End();
                }

                // _counter++;
                // _currentPathCreator = _pathCreators[_counter];
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

        public void SetPathCreator(PathCreator pathCreator)
        {
            Debug.Log("setting path creators");
            _pathCreator = pathCreator;
        }

        public float GetTime()
        {
            return _time;
        }
    }
}