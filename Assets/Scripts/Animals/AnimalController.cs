using UnityEngine;

namespace Animals
{
    public class AnimalController : MonoBehaviour
    {
        public GameObject animal;
        
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Time = Animator.StringToHash("Time");
        private Animator _animator;
        private AnimalMovement _movement;
        private FadeOut _fadeOut;


        void Start()
        {
            _animator = GetComponent<Animator>();
            _movement = animal.GetComponent<AnimalMovement>();
            _fadeOut = animal.GetComponent<FadeOut>();
        }

        public void TriggerMove()
        {
            _animator.SetTrigger(Run);
            // _animator.SetFloat(Time, 1f);

            _movement.enabled = true;
        }

        public void TriggerFadeOut()
        {
            _fadeOut.enabled = true;
        }
    }
}
