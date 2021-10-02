using UnityEngine;

namespace UI
{
    public class UIAnimator : MonoBehaviour
    {

        private Animator _animator;
        private static readonly int Enable = Animator.StringToHash("Enable");

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animator.SetTrigger(Enable);
        }
    }
}
