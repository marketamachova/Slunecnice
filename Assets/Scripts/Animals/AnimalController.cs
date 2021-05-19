using UnityEngine;
using UnityEngine.Serialization;

namespace Animals
{
    public enum MovingType
    {
        Walk,
        Run
    }

    public class AnimalController : MonoBehaviour
    {
        [SerializeField] private bool moving;
        [SerializeField] private bool destroyable;
        [SerializeField] private bool walker;
        [SerializeField] private MovingType movingType;

        private string _movingState;
        private readonly string _idleState = "idle";

        private Animator _animator;
        private AnimalMovement _movement;
        private FadeOut _fadeOut;
        private AudioSource _audioSource;


        void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<AnimalMovement>();
            _fadeOut = GetComponent<FadeOut>();
            _audioSource = GetComponent<AudioSource>();
            _movingState = movingType.ToString().ToLower();
        }

        public void TriggerMove()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<AnimalMovement>();

            _animator.Play(_movingState);
            if (_movement)
            {
                _movement.enabled = true;
            }
        }

        public void TriggerSound()
        {
            if (_audioSource == null)
            {
                return;
            }

            _audioSource.PlayOneShot(_audioSource.clip);
        }

        public void TriggerStop()
        {
            _animator.Play(_idleState);
            if (destroyable)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        public void TriggerFadeOut()
        {
            _fadeOut.enabled = true;
        }

        public bool GetMoving()
        {
            return moving;
        }
    }
}