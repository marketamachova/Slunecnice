using System;
using PathCreation;
using UnityEngine;

namespace Cart
{
    public class CartMovement : MonoBehaviour
    {
        public GameObject cart;
        private PathCreator _pathCreator;
        public float speed;
        private float _distance;
        private Animator _animator;
        private static readonly int Drive = Animator.StringToHash("Drive");

        void Start()
        {
            _pathCreator = GameObject.FindWithTag("PathCreator").GetComponent<PathCreator>();
            _animator = cart.GetComponent<Animator>();
            _animator.SetTrigger(Drive);
            cart.transform.position = _pathCreator.path.GetPoint(0);
        }

        void Update()
        {
            _distance += speed * Time.deltaTime;
            cart.transform.position = _pathCreator.path.GetPointAtDistance(_distance);
            cart.transform.rotation = _pathCreator.path.GetRotationAtDistance(_distance);
        }
    }
}