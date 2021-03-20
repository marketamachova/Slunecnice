using System;
using System.Collections;
using System.Collections.Generic;
using Cart;
using UnityEngine;

namespace Player
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public List<GameObject> player;
        [SerializeField] private GameObject cart;
        [SerializeField] private GameObject finalUI;
        
        public readonly List<PlayerMovement> PlayerMovementScripts = new List<PlayerMovement>();
        private CartMovement _cartMovement;
        private AudioSource _cartAudio;
        private Fader _fader;
        
        private static readonly int Stop = Animator.StringToHash("Stop");

        private void Awake()
        {
            foreach (var o in player)
            {
                PlayerMovementScripts.Add(o.GetComponent<PlayerMovement>());
            }
            
            _cartMovement = cart.GetComponent<CartMovement>();
            _cartAudio = cart.GetComponent<AudioSource>();
            // _fader = GetComponent<Fader>();
        }

        public IEnumerator Start()
        {
            // yield return StartCoroutine(_fader.FadeCoroutine());
            yield return new WaitForSecondsRealtime(4);
            // yield return StartCoroutine(InitialCoroutine());
        }

        IEnumerator InitialCoroutine()
        {
            PlayerMovementScripts.ForEach(Enable);
            _cartMovement.enabled = true;
            _cartAudio.Play();
            yield return null;
        }
        
        public void StartMovement()
        {
            Debug.Log("game controller starting movement");
            PlayerMovementScripts.ForEach(Enable);
        }
        
        public void PauseMovement()
        {
            PlayerMovementScripts.ForEach(Disable);
        }

        public void End()
        {
            PlayerMovementScripts.ForEach(Disable);
            StopCart();
            DisplayUI();
        }
        
        private void Enable(PlayerMovement script)
        {
            script.enabled = true;
        }
        
        private void Disable(PlayerMovement script)
        {
            script.enabled = false;
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

        private void OnSceneLoaded()
        {
            //center player
        }
    }
}
