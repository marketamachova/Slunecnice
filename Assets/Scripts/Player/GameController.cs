using System.Collections;
using System.Collections.Generic;
using Cart;
using UnityEngine;

namespace Player
{
    public class GameController : MonoBehaviour
    {
        public List<GameObject> player;
        public GameObject countDown;
        public GameObject cart;
        private readonly List<PlayerMovement> _playerMovementScripts = new List<PlayerMovement>();
        private CartMovement _cartMovement;
        private CountDownHandler _countDownHandler;
        private static readonly int Stop = Animator.StringToHash("Stop");

        void Start()
        {
            foreach (var o in player)
            {
                _playerMovementScripts.Add(o.GetComponent<PlayerMovement>());
            }
            _countDownHandler = countDown.GetComponent<CountDownHandler>();
            _cartMovement = cart.GetComponent<CartMovement>();
        
            StartCoroutine(CountDown());
        }
    
    
        IEnumerator CountDown()
        {
            while (_countDownHandler.GetTextInt() > 0)
            {
                yield return new WaitForSecondsRealtime(2);
                _countDownHandler.Decrement();

            }

            _countDownHandler.Destroy();
            yield return new WaitForSecondsRealtime(1);
            
            _playerMovementScripts.ForEach(Enable);
            _cartMovement.enabled = true;
        }

        private void Enable(PlayerMovement script)
        {
            script.enabled = true;
        }
        
        private void Disable(PlayerMovement script)
        {
            script.enabled = false;
        }

        public void End()
        {
            _playerMovementScripts.ForEach(Disable);
            _cartMovement.enabled = false;
            var animator = cart.GetComponent<Animator>();
            animator.SetTrigger(Stop);
            DisplayUI();
        }

        private void DisplayUI()
        {
        
        }

    }
}
