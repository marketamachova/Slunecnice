using UnityEngine;

namespace Animals
{
    public class AnimalController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Run = Animator.StringToHash("Run");
        public GameObject animal;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            float move = Input.GetAxis("Vertical");
        }

        public void TriggerMove()
        {
            Debug.Log("Trigger Move in AnimalController called");
            var movement = animal.GetComponent<AnimalMovement>();

            _animator.SetTrigger(Run);
            _animator.CrossFadeInFixedTime(Run, 0.2f);
            movement.enabled = true;
        }
    }
}
