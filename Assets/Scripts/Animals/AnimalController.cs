using UnityEngine;

namespace Animals
{
    public class AnimalController : MonoBehaviour
    {
        public GameObject animal;
        public bool moving;
        [SerializeField] private bool destroyable;

        private static readonly string RunState = "run";
        private static readonly string IdleState = "idle";

        private Animator _animator;
        private AnimalMovement _movement;
        private FadeOut _fadeOut;
        private AudioSource _audioSource;


        void Start()
        {
            _animator = animal.GetComponent<Animator>();
            _movement = animal.GetComponent<AnimalMovement>();
            _fadeOut = animal.GetComponent<FadeOut>();
            _audioSource = animal.GetComponent<AudioSource>();
        }

        public void TriggerMove()
        {
            _animator.Play(moving ? RunState : RunState);
            _movement.enabled = true;
        }

        public void TriggerSound()
        {
            if (_audioSource == null)
            {
                Debug.Log("Audio source null");
                return;
            }

            _audioSource.PlayOneShot(_audioSource.clip);
        }

        public void TriggerStop()
        {
            _animator.Play(IdleState);
            if (destroyable)
            {
                Destroy(animal);
            }
        }

        public void TriggerFadeOut()
        {
            _fadeOut.enabled = true;
        }
    }
}