using UnityEngine;

namespace UI
{
    public class UIAnimator : MonoBehaviour
    {

        private Animator _animator;
        private static readonly int Enable = Animator.StringToHash("Enable");
        
        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(Enable);
        }
    }
}
