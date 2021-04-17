using System;
using System.Linq;
using Mirror;
using Player;
using Scenes;
using UI;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Network
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SyncVar(hook = "SetMobile")] public bool mobile = false;

        [SyncVar(hook = "ChangeScene")] public string chosenWorld;

        [SyncVar(hook = "SetPlayerMoving")] public bool playerMoving = false;

        [SyncVar(hook = "SetSpeed")] public int speed = 2;

        [SyncVar(hook = "SetTimePlaying")] public float timePlaying = 0f;

        [SyncVar(hook = "SkipCalibration")] public bool skipCalibration;

        [SyncVar(hook = "SetCalibrationComplete")]
        public bool calibrationComplete;
        
        [SyncVar(hook = "TriggerTimeSync")]
        public bool triggerTimeSync;

        [SyncVar(hook = "GoToLobby")] public bool goToLobby;

        private BaseUIController _uiController;
        private GameController _gameController;
        private BaseController _controller;
        private SceneLoader _sceneLoader;
        private NetworkPlayer[] _networkPlayers;

        public event Action OnCalibrationComplete;

        private void Awake()
        {
            _controller = FindObjectOfType<BaseController>();
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _gameController = FindObjectOfType<GameController>();
            _uiController = FindObjectOfType<BaseUIController>();
            _networkPlayers = FindObjectsOfType<NetworkPlayer>();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (mobile && isLocalPlayer)
            {
                _controller.OnDisconnect();
            }
            else
            {
                if (!playerMoving)
                {
                    playerMoving = true;
                }
            }
        }

        /**
         * assign network player to mobile client
         */
        [TargetRpc]
        public void UpdateSceneConnected()
        {
            _controller.AssignPlayers();

            if (SceneManager.GetActiveScene().name == "AppOffline" && isLocalPlayer)
            {
                Debug.Log("update scene connected (MOBILE ONLY)");
                CmdSetMobile(true);
                ((MobileController) _controller).AssignPlayer(this); // mozna presunout do onstrtlocalplayer
            }
        }

        /**
         * CALLBACKS
         */
        public void ChangeScene(string oldScene, string newScene)
        {
            if (String.IsNullOrEmpty(newScene))
            {
                return;
            }

            goToLobby = false;
            DontDestroyOnLoad(this);
            string sceneToLoad = newScene;
            Debug.Log("Change sceen in network player, mobile: " + mobile);
            if (isLocalPlayer)
            {
                if ((mobile || SceneManager.GetActiveScene().name == "AppOffline"))
                {
                    Debug.Log("mobile true");
                    sceneToLoad = "PlsMobile"; //TODO
                }

                _sceneLoader.LoadScene(sceneToLoad, true);
            }
        }

        private void SetMobile(bool oldValue, bool mobile)
        {
            Debug.Log("mobile noe: " + mobile);
        }

        public void GoToLobby(bool oldValue, bool exit)
        {
            if (exit)
            {
                playerMoving = false;
                chosenWorld = String.Empty;

                if (_networkPlayers.Length < 2)
                {
                    AssignNetworkPlayers();
                }
                foreach (var networkPlayer in _networkPlayers)
                {
                    networkPlayer.CmdSyncTimePlaying(0f);
                }
                
                if (mobile && isLocalPlayer)
                {
                    _controller.OnGoToLobby();
                }
                else if (!mobile && isLocalPlayer)
                {
                    Debug.Log("network player go to lobby");
                    _gameController = FindObjectOfType<GameController>();
                    _gameController.GoToLobby();
                }
            }
        }

        //volano serverem na clientech
        public void SetPlayerMoving(bool oldValue, bool moving)
        {
            Debug.Log("set player moving in Network pLayer, moving: " + moving);

            if (!mobile && isLocalPlayer)
            {
                AssignGameController(); //TODO nejak jinak mozna smazat

                if (moving)
                {
                    Debug.Log(_gameController);
                    _gameController.StartMovement();
                }
                else
                {
                    _gameController.PauseMovement();
                }
            }
        }

        public void SetSpeed(int oldValue, int movingSpeed)
        {
            if (isLocalPlayer)
            {
                Debug.Log("good");
                if (!mobile)
                {
                    _gameController.SetMovementSpeed(movingSpeed);
                }
            }
        }

        public void SkipCalibration(bool oldValue, bool skip)
        {
            Debug.Log("set player skip calibration CALLBACK in Network pLayer, skip: " + skip);
            if (!mobile)
            {
                _controller.SkipCalibration();
                // ((VRLobbyController) _controller).GetCartCreator().SkipCalibration();
            }
        }

        //called automatically, after calibrationComplete changes
        private void SetCalibrationComplete(bool oldValue, bool complete)
        {
            Debug.Log("Set calibration complete callback nETWORK PLAYER");
            OnCalibrationComplete?.Invoke(); //ma si to prevzit mobile controller (MobileController)
        }

        private void SetTimePlaying(float oldValue, float playTime)
        {
            Debug.Log("set time playing in network player");
            if (mobile && isLocalPlayer)
            {
                ((UIControllerMobile) _uiController).UpdateTimer(playerMoving, playTime);
            }
        }

        private void TriggerTimeSync(bool oldValue, bool timeSyncTrigger)
        {
            if (timeSyncTrigger && !mobile && isLocalPlayer)
            {
                Debug.Log("VR local player cmd time sync");
                if (!_gameController)
                {
                    AssignGameController();
                }

                if (_networkPlayers.Length < 2)
                {
                    AssignNetworkPlayers();
                }
                foreach (var networkPlayer in _networkPlayers)
                {
                    networkPlayer.CmdSyncTimePlaying(_gameController.GetTimePlaying());
                }
            }
        }


        /**
         * COMMANDS
         */

        //mozna ze to coje ted v networkManager se da dat i sem jako serverova strana
        [Command(requiresAuthority = false)] //require authority
        public void CmdHandleSelectedWorld(string sceneName)
        {
            Debug.Log(chosenWorld);
            if (String.IsNullOrEmpty(chosenWorld))
            {
                chosenWorld = sceneName; //changing syncvar as cmd results in server synchronising all clients
            }
        }

        //volano Controllerem
        [Command(requiresAuthority = false)] //require authority
        public void CmdSetPlayerMoving(bool moving)
        {
            Debug.Log("CMD set player moving in Network pLayer, moving: " + moving);

            playerMoving = moving;
        }

        [Command(requiresAuthority = false)]
        public void CmdSkipCalibration(bool skip)
        {
            Debug.Log("CMD sskip calibration in Network pLayer, skipCalibraion: " + skip);
            skipCalibration = true;
        }

        [Command(requiresAuthority = false)] //require authority
        public void CmdGoToLobby()
        {
            goToLobby = true;
        }


        [Command(requiresAuthority = false)]
        public void CmdSetCalibrationComplete(bool complete)
        {
            Debug.Log("CMD set calibrationComplete in Network pLayer, complete: " + complete);
            calibrationComplete = complete;
        }

        [Command]
        public void CmdSetMobile(bool mobile)
        {
            this.mobile = mobile;
        }

        [Command(requiresAuthority = false)]
        public void CmdSetSpeed(int movingSpeed)
        {
            this.speed = movingSpeed;
        }

        [Command(requiresAuthority = false)]
        public void CmdSyncTimePlaying(float timePlayingValue)
        {
            Debug.Log("CmdSyncTimePlaying " + timePlayingValue);
            timePlaying = timePlayingValue;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdTriggerTimeSync()
        {
            Debug.Log("CmdTriggerTimeSync " + true);

            triggerTimeSync = true;
        }

        public GameController GetGameController()
        {
            if (_gameController == null)
            {
                AssignGameController();
            }

            return _gameController;
        }
        
        private void AssignGameController()
        {
            _gameController = FindObjectOfType<GameController>();
        }

        private void AssignNetworkPlayers()
        {
            _networkPlayers = FindObjectsOfType<NetworkPlayer>();
        }

    }
}