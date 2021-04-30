using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [FormerlySerializedAs("Speed")] [SerializeField]
        public float speed = 2f;

        [SerializeField] private EndOfPathInstruction endOfPathInstruction;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, 0);
        [SerializeField] private bool rotateCamera = true;
        [SerializeField] public bool reverse;

        private Animator _animator;
        private PathCreator _pathCreator;
        private GameController _controller;
        private float _distance;
        private bool _shiftCamera = false;
        private float _time;

        private static readonly int Drive = Animator.StringToHash("Drive");

        void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("NetworkCamera");
            }

            reverse = SceneManager.GetActiveScene().name.Equals("WinterScene");
        }

        void Start()
        {
            _controller = FindObjectOfType<GameController>();
            _pathCreator = GameObject.FindWithTag("PathCreator").GetComponent<PathCreator>();

            var startingPos = _pathCreator.path.GetPoint(0) + offset;
            if (reverse)
            {
                startingPos = _pathCreator.path.GetPoint(_pathCreator.path.NumPoints -1) + offset;
            }

            player.transform.position = startingPos;
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

            if (reverse)
            {
                _distance -= speed * Time.deltaTime;
            }
            else
            {
                _distance += speed * Time.deltaTime;
            }

            player.transform.position =
                _pathCreator.path.GetPointAtDistance(_distance + cameraOffset, endOfPathInstruction) + offset;

            if (rotateCamera)
            {
                player.transform.rotation = _pathCreator.path.GetRotationAtDistance(_distance, endOfPathInstruction);
            }

            if (_distance >= _pathCreator.path.length)
            {
                Debug.Log("calling controller.End");
                Debug.Log("controller" + _controller);
                _controller.End();
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