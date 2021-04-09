using System;
using System.Collections;
using System.Collections.Generic;
using Cart;
using PathCreation;
using Scenes;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Player
{
    public class GameController : MonoBehaviour
    {
        public List<GameObject> player = new List<GameObject>();
        private SceneController _sceneController;
        private PathCreator _pathCreator;

         private PlayerMovement[] _playerMovementScripts;
        private GameObject _cart;
        private GameObject _player;
        private CartMovement _cartMovement;
        private Animator _cartAnimator;
        private AudioSource _cartAudio;
        private Fader _fader;
        private string _currentScene = "MainScene";
        private bool _mobile;

        private static readonly int Stop = Animator.StringToHash("Stop");
        private static readonly int Drive = Animator.StringToHash("Drive");
        
        //TODO refactor
        private void Awake()
        {
            _mobile = SceneManager.GetSceneAt(0).name == "AppOffline";
            
            _cart = GameObject.FindWithTag("Cart");
            _player = GameObject.FindWithTag("NetworkCamera");
            _sceneController = GameObject.FindObjectOfType<SceneController>();
            _pathCreator = FindObjectOfType<PathCreator>();
                
            player.Add(_player);

            _playerMovementScripts = FindObjectsOfType<PlayerMovement>();
            foreach (var script in _playerMovementScripts)
            {
                script.SetPathCreator(_pathCreator);
            }
            
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
            
            foreach (var script in _playerMovementScripts)
            {
                Enable(script);
            }
            
            if (_cart != null)
            {
                StartCart();
            }
        }

        public void PauseMovement()
        {
            foreach (var script in _playerMovementScripts)
            {
                Disable(script);
            }
            if (_cart != null)
            {
                StopCart();
            }
        }

        public void End()
        {
            foreach (var script in _playerMovementScripts)
            {
                Disable(script);
            }
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
            _sceneController.MovePlayersAtStartingPositionLobby();
            SceneManager.UnloadSceneAsync(_currentScene);
        }

        private void OnSceneLoaded()
        {
            //center player
        }
    }
}