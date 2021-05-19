using PathCreation;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] public float speed = 2f;
        [SerializeField] private EndOfPathInstruction endOfPathInstruction;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, 0);
        [SerializeField] private bool rotateCamera = true;

        private Animator _animator;
        private PathCreator _pathCreator;
        private VRController _controller;
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
        }

        void Start()
        {
            _controller = FindObjectOfType<VRController>();
            _pathCreator = GameObject.FindWithTag("PathCreator").GetComponent<PathCreator>();

            var startingPos = _pathCreator.path.GetPoint(0) + offset;

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
            _distance += speed * Time.deltaTime;

            player.transform.position =
                _pathCreator.path.GetPointAtDistance(_distance + cameraOffset, endOfPathInstruction) + offset;

            if (rotateCamera)
            {
                player.transform.rotation = _pathCreator.path.GetRotationAtDistance(_distance, endOfPathInstruction);
            }

            if (_distance >= _pathCreator.path.length)
            {
                _controller.End();
            }
        }
        
        public void SetPathCreator(PathCreator pathCreator)
        {
            _pathCreator = pathCreator;
        }

        public float GetTime()
        {
            return _time;
        }
    }
}