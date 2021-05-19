using System.Collections;
using System.Collections.Generic;
using Network;
using PathCreation;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    /**
     * Controller managing events in VR travelling experience
     */
    public class VRController : MonoBehaviour
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
            _cart = GameObject.FindWithTag(GameConstants.Cart);
            _player = GameObject.FindWithTag(GameConstants.NetworkCamera);
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
        }

        public IEnumerator Start()
        {
            if (_networkManager.numPlayers == 1)
            {
                yield return new WaitForSecondsRealtime(4);
                TriggerPlayerMoving();
            }
        }


        public void StartMovement()
        {
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
            StartCoroutine(GoToLobbyCoroutine());
        }


        /**
         * coroutine of VR player returning to lobby
         * 1. Waits for defined waiting time
         * 2. Sets Lobby as active scene
         * 3. Moves player at accurate position in Lobby
         * 4. calls async scene unloading
         */
        private IEnumerator GoToLobbyCoroutine()
        {
            yield return new WaitForSecondsRealtime(GameConstants.ReturnToLobbyWaitingTime);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameConstants.VROffline));

            _sceneController.MovePlayersAtStartingPositionLobby();
            
            _sceneLoader.UnloadScene();
        }

        public void SetMovementSpeed(int speed)
        {

            if (_playerMovementScripts.Length > 0) //TODO
            {
                foreach (var playerMovement in _playerMovementScripts)
                {
                    playerMovement.speed = speed;
                    _customSpeed = speed;
                }
            }
            else
            {
                _customSpeed = speed;
            }
        }

        /**
         * return time (float) spent in an ongoing VR experience
         */
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
            _networkPlayer.CmdSetPlayerMoving(true);
        }
    }
}