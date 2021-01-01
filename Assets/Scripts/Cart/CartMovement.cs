using PathCreation;
using UnityEngine;

namespace Cart
{
    public class CartMovement : MonoBehaviour
    {
        public GameObject cart;
        public PathCreator pathCreator;
        public float speed;
        private float _distance;
        private Animator _animator;
        private static readonly int Drive = Animator.StringToHash("Drive");

        void Start()
        {
            _animator = cart.GetComponent<Animator>();
            _animator.SetTrigger(Drive);
            cart.transform.position = pathCreator.path.GetPoint(0);
        }
    
        void Update()
        {
            _distance += speed * Time.deltaTime;
            cart.transform.position = pathCreator.path.GetPointAtDistance(_distance);
            cart.transform.rotation = pathCreator.path.GetRotationAtDistance(_distance);
        }
    }
}
