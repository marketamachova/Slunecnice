using System;
using System.Collections;
using System.Collections.Generic;
using Cart;
using UnityEngine;

namespace Player
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> player;
        [SerializeField] private GameObject cart;
        [SerializeField] private GameObject finalUI;
        
        private readonly List<PlayerMovement> _playerMovementScripts = new List<PlayerMovement>();
        private CartMovement _cartMovement;
        private AudioSource _cartAudio;
        private Fader _fader;
        
        private static readonly int Stop = Animator.StringToHash("Stop");

        private void Awake()
        {
            foreach (var o in player)
            {
                _playerMovementScripts.Add(o.GetComponent<PlayerMovement>());
            }
            
            _cartMovement = cart.GetComponent<CartMovement>();
            _cartAudio = cart.GetComponent<AudioSource>();
            // _fader = GetComponent<Fader>();
        }

        public IEnumerator Start()
        {
            // yield return StartCoroutine(_fader.FadeCoroutine());
            yield return new WaitForSecondsRealtime(4);
            yield return StartCoroutine(InitialCoroutine());
        }
        

        IEnumerator InitialCoroutine()
        {
            _playerMovementScripts.ForEach(Enable);
            _cartMovement.enabled = true;
            _cartAudio.Play();
            yield return null;
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
            StopCart();
            DisplayUI();
        }

        private void DisplayUI()
        {
            finalUI.SetActive(true);
        }

        private void StopCart()
        {
            var animator = cart.GetComponent<Animator>();
            animator.SetTrigger(Stop);
            _cartMovement.enabled = false;
            _cartAudio.Stop();
        }
    }
}
