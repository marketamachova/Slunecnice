using System;
using System.Collections;
using System.Collections.Generic;
using Cart;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public List<GameObject> player;
        [SerializeField] private SceneController sceneController;
        [SerializeField] private List<PathCreator> pathCreators;

        public readonly List<PlayerMovement> PlayerMovementScripts = new List<PlayerMovement>();
        private GameObject _cart;
        private GameObject _player;
        private CartMovement _cartMovement;
        private Animator _cartAnimator;
        private AudioSource _cartAudio;
        private Fader _fader;
        private string _currentScene = "MainScene";

        private static readonly int Stop = Animator.StringToHash("Stop");
        private static readonly int Drive = Animator.StringToHash("Drive");

        private void Awake()
        {
            _cart = GameObject.FindWithTag("Cart");
            _player = GameObject.FindWithTag("NetworkCamera");
            player.Add(_player);
            Debug.Log(player.Count);

            foreach (var o in player)
            {
                PlayerMovementScripts.Add(o.GetComponent<PlayerMovement>());
            }

            PlayerMovementScripts.ForEach(script => script.SetPathCreator(pathCreators));

            if (_cart != null)
            {
                _cartMovement = _cart.GetComponent<CartMovement>();
                _cartAudio = _cart.GetComponent<AudioSource>();
                _cartAnimator = _cart.GetComponent<Animator>();
            }

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
            StartMovement();
            yield return null;
        }

        public void StartMovement()
        {
            Debug.Log("game controller starting movement");
            PlayerMovementScripts.ForEach(Debug.Log);
            PlayerMovementScripts.ForEach(Enable);
            if (_cart != null)
            {
                StartCart();
            }
        }

        public void PauseMovement()
        {
            PlayerMovementScripts.ForEach(Disable);
            if (_cart != null)
            {
                StopCart();
            }
        }

        public void End()
        {
            PlayerMovementScripts.ForEach(Disable);
            if (_cart != null)
            {
                StopCart();
            }

            StartCoroutine(GoToLobby());
        }

        private void Enable(PlayerMovement script)
        {
            script.enabled = true;
        }

        private void Disable(PlayerMovement script)
        {
            script.enabled = false;
        }

        private void StartCart()
        {
            _cartAnimator.SetTrigger(Drive);
            _cartAudio.Play();
        }


        private void StopCart()
        {
            _cartAnimator = _cart.GetComponent<Animator>();
            _cartAnimator.SetTrigger(Stop);
            _cartAudio.Stop();
        }

        private IEnumerator GoToLobby()
        {
            yield return new WaitForSecondsRealtime(3);
            DontDestroyOnLoad(this);
            sceneController.MovePlayersAtStartingPositionLobby();
            SceneManager.UnloadSceneAsync(_currentScene);
        }

        private void OnSceneLoaded()
        {
            //center player
        }
    }
}