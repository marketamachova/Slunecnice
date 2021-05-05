using System;
using System.Collections;
using System.Collections.Generic;
using Cart;
using Mirror;
using Network;
using PathCreation;
using Scenes;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    public class GameController : MonoBehaviour
    {
        public List<GameObject> player = new List<GameObject>();
        private SceneController _sceneController;
        private SceneLoader _sceneLoader;
        private PathCreator _pathCreator;
        private MyNetworkManager _networkManager;
        private PlayerMovement[] _playerMovementScripts;
        private GameObject _cart;
        private GameObject _player;
        private Animator _cartAnimator;
        private AudioSource _cartAudio;
        private NetworkPlayer _networkPlayer;
        private Fader _fader;

        private string _currentScene;

        private int _customSpeed = 10;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Drive = Animator.StringToHash("Drive");

        private void Awake()
        {
            _networkPlayer = FindObjectOfType<NetworkPlayer>();
            _currentScene = SceneManager.GetActiveScene().name;

            _customSpeed = _networkPlayer.speed;
            _cart = GameObject.FindWithTag("Cart");
            _player = GameObject.FindWithTag("NetworkCamera");
            _sceneController = GameObject.FindObjectOfType<SceneController>();
            _sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
            _pathCreator = FindObjectOfType<PathCreator>();
            _networkManager = FindObjectOfType<MyNetworkManager>();

            player.Add(_player);

            _playerMovementScripts = FindObjectsOfType<PlayerMovement>();
            foreach (var script in _playerMovementScripts)
            {
                script.SetPathCreator(_pathCreator);
                script.speed = _customSpeed;
            }

            if (_cart != null)
            {
                _cartAudio = _cart.GetComponent<AudioSource>();
                _cartAnimator = _cart.GetComponent<Animator>();
            }

            _networkManager.OnMobileClientDisconnectAction += TriggerPlayerMoving;
            _networkManager.OnClientDisconnectAction += TriggerPlayerMoving;
            // _fader = GetComponent<Fader>(
        }

        public IEnumerator Start()
        {
            // yield return StartCoroutine(_fader.FadeCoroutine());

            if (_networkManager.numPlayers == 1)
            {
                yield return new WaitForSecondsRealtime(4);
                TriggerPlayerMoving();
            }
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

            Debug.Log("calling CmdGoToLobby");

            var networkPlayers = FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in networkPlayers)
            {
                networkPlayer.CmdGoToLobby();
            }
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
            _cartAnimator.Play(Drive);
            _cartAudio.Play();
        }


        private void StopCart()
        {
            _cartAnimator = _cart.GetComponent<Animator>();
            _cartAnimator.Play(Idle);
            _cartAudio.Stop();
        }

        public void GoToLobby()
        {
            Debug.Log("game controller go to lobby");
            StartCoroutine(GoToLobbyCoroutine());
        }


        private IEnumerator GoToLobbyCoroutine()
        {
            yield return new WaitForSecondsRealtime(2);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("VROffline"));

            _sceneController.MovePlayersAtStartingPositionLobby();
            // yield return new WaitForSecondsRealtime(1);

            Debug.Log("Unloading scene");
            
            _sceneLoader.UnloadScene();
        }

        public void SetMovementSpeed(int speed)
        {

            if (_playerMovementScripts.Length > 0)
            {
                foreach (var playerMovement in _playerMovementScripts)
                {
                    playerMovement.speed = speed;
                    _customSpeed = speed;
                }
            }
            else
            {
                Debug.Log("custom speed set");
                _customSpeed = speed;
            }
        }

        public float GetTimePlaying()
        {
            if (_playerMovementScripts.Length > 0)
            {
                return _playerMovementScripts[0].GetTime();
            }

            return 0f;
        }

        private void TriggerPlayerMoving()
        {
            Debug.Log("trigger player move");
            _networkPlayer.CmdSetPlayerMoving(true);
        }
    }
}