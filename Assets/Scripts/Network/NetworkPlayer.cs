using System;
using Mirror;
using Player;
using Scenes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SyncVar] public bool mobile = false;

        [SyncVar(hook = "ChangeScene")] public string chosenWorld;

        [SyncVar(hook = "SetPlayerMoving")] public bool playerMoving = false;

        [SyncVar(hook = "SetSpeed")] public int speed = 2;

        [SyncVar(hook = "SetTimePlaying")] public float timePlaying = 0f;

        [SyncVar(hook = "SkipCalibration")] public bool skipCalibration;

        [SyncVar(hook = "SetCalibrationComplete")]
        public bool calibrationComplete;

        [SyncVar(hook = "TriggerTimeSync")] public bool triggerTimeSync;

        [SyncVar(hook = "GoToLobby")] public bool goToLobby = true;

        [SyncVar(hook = "OnWorldLoaded")] public bool worldLoaded;


        private BaseUIController _uiController;
        private VRController _vrController;
        private BaseController _controller;
        private SceneLoader _sceneLoader;
        private NetworkPlayer[] _networkPlayers;

        public event Action OnCalibrationComplete;
        public event Action OnSceneLoadedAction;

        private void Awake()
        {
            _controller = FindObjectOfType<BaseController>();
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _vrController = FindObjectOfType<VRController>();
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
                    foreach (var networkPlayer in _networkPlayers)
                    {
                        if (networkPlayer.isActiveAndEnabled)
                        {
                            networkPlayer.CmdSetPlayerMoving(true);
                        }
                    }
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

            if (SceneManager.GetActiveScene().name == GameConstants.AppOffline && isLocalPlayer)
            {
                CmdSetMobile(true);
                ((MobileController) _controller).AssignPlayer(this);
            }
        }

        /**
         * CALLBACKS
         *
         * methods below all called whenever a specific SyncVar is changed 
         */
        
        /**
         * triggers loading a scene (additively)
         * if VR: loads the the chosen scene
         * if mobile: loads a mobile version of the chosen scene 
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
            if (isLocalPlayer)
            {
                if (mobile || SceneManager.GetActiveScene().name == GameConstants.AppOffline)
                {
                    sceneToLoad = sceneToLoad + "Mobile";
                }

                _sceneLoader.LoadScene(sceneToLoad, true);
            }
        }

        /**
         * if goToLobby true: set SyncVars to default values and synchronise them across players
         * call appropriate methods at mobile and VR players' controllers 
         */
        public void GoToLobby(bool oldValue, bool exit)
        {
            if (exit)
            {
                playerMoving = false;
                chosenWorld = String.Empty;
                triggerTimeSync = false;
                timePlaying = 0f;

                if (_networkPlayers.Length < 2)
                {
                    AssignNetworkPlayers();
                }

                foreach (var networkPlayer in _networkPlayers)
                {
                    networkPlayer.CmdSetPlayerMoving(false);
                    networkPlayer.CmdHandleSelectedWorld("");
                    networkPlayer.CmdSyncTimePlaying(0f);
                    networkPlayer.CmdTriggerTimeSync(false);
                }

                if (isLocalPlayer)
                {
                    if (!mobile)
                    {
                        if (!_vrController)
                        {
                            AssignGameController();
                        }

                        _vrController.GoToLobby();
                        _controller.OnGoToLobby();
                    }

                    _controller.OnGoToLobby();
                }
            }
        }

        public void SetPlayerMoving(bool oldValue, bool moving)
        {
            if (!mobile && isLocalPlayer && !goToLobby)
            {
                if (!_vrController)
                {
                    AssignGameController();
                }

                if (moving)
                {
                    _vrController.StartMovement();
                }
                else
                {
                    _vrController.PauseMovement();
                }
            }
        }

        public void SetSpeed(int oldValue, int movingSpeed)
        {
            if (isLocalPlayer && !mobile)
            {
                if (!_vrController)
                {
                    AssignGameController();
                }
                _vrController.SetMovementSpeed(movingSpeed);
            }
        }

        public void SkipCalibration(bool oldValue, bool skip)
        {
            if (!mobile)
            {
                _controller.SkipCalibration();
            }
        }

        private void SetCalibrationComplete(bool oldValue, bool complete)
        {
            OnCalibrationComplete?.Invoke();
        }

        /**
         * syncs the time spent in an ongoing VR experience in case the mobile player joins during an ongoing VR experience
         */
        private void SetTimePlaying(float oldValue, float playTime)
        {
            Debug.Log(SceneManager.GetSceneAt(0).name);
            if (SceneManager.GetSceneAt(0).name.Equals(GameConstants.AppOffline))
            {
                ((UIControllerMobile) _uiController).UpdateTimer(playerMoving, playTime);
            }
        }

        /**
         * trigger synchronisation of the time spent in an ongoing VR experience
         */
        private void TriggerTimeSync(bool oldValue, bool timeSyncTrigger)
        {
            if (timeSyncTrigger && !mobile && isLocalPlayer)
            {
                if (!_vrController)
                {
                    AssignGameController();
                }

                var currentTimePlaying = _vrController.GetTimePlaying();

                if (_networkPlayers.Length < 2)
                {
                    AssignNetworkPlayers();
                }

                foreach (var networkPlayer in _networkPlayers)
                {
                    RpcSetTimePlaying(currentTimePlaying);
                    networkPlayer.timePlaying = currentTimePlaying;
                    networkPlayer.triggerTimeSync = false;
                }
            }
        }

        /**
         * invokes event when world is loaded at the VR client
         */
        private void OnWorldLoaded(bool oldValue, bool loaded)
        {
            if (loaded && !mobile)
            {
                OnSceneLoadedAction?.Invoke();
            }
        }


        /**
         * COMMANDS
         *
         * methods below change the values of SyncVars 
         * - called from clients but executed on the server
         * - requiresAuthority set to false to be able to sync SyncVars across all players  
         */
        [Command(requiresAuthority = false)]
        public void CmdHandleSelectedWorld(string sceneName)
        {
            if (String.IsNullOrEmpty(chosenWorld))
            {
                chosenWorld = sceneName; //changing syncvar as cmd results in server synchronising all clients
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdSetPlayerMoving(bool moving)
        {
            if (!goToLobby)
            {
                playerMoving = moving;
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdSkipCalibration(bool skip)
        {
            skipCalibration = true;
        }

        [Command(requiresAuthority = false)]
        public void CmdGoToLobby()
        {
            goToLobby = true;
        }


        [Command(requiresAuthority = false)]
        public void CmdSetCalibrationComplete(bool complete)
        {
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
            timePlaying = timePlayingValue;
        }

        [Command(requiresAuthority = false)]
        public void CmdTriggerTimeSync(bool trigger)
        {
            triggerTimeSync = trigger;
        }

        [Command]
        public void CmdSetWorldLoaded(bool loaded)
        {
            worldLoaded = loaded;
        }

        [TargetRpc]
        public void RpcSetTimePlaying(float time)
        {
            timePlaying = time;
        }


        private void AssignGameController()
        {
            _vrController = FindObjectOfType<VRController>();
        }

        private void AssignNetworkPlayers()
        {
            _networkPlayers = FindObjectsOfType<NetworkPlayer>();
        }
    }
}