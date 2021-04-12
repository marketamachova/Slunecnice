using System;
using System.Linq;
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
        [SerializeField] public bool mobile = false;

        [SyncVar(hook = "ChangeScene")] public string chosenWorld;

        [SyncVar(hook = "SetPlayerMoving")] public bool playerMoving = false;

        [SyncVar(hook = "SkipCalibration")] public bool skipCalibration;

        [SyncVar(hook = "SetCalibrationComplete")]
        public bool calibrationComplete;

        [SyncVar(hook = "GoToLobby")]
        public bool goToLobby;

        private BaseUIController _uiController;
        private GameController _gameController;
        private BaseController _controller;
        private SceneLoader _sceneLoader;

        public event Action OnCalibrationComplete;

        private void Awake()
        {
            _controller = FindObjectOfType<BaseController>();
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _gameController = FindObjectOfType<GameController>();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (mobile)
            {
                _controller.OnDisconnect();
            }
        }

        // public override void OnStartLocalPlayer()
        // {
        //     base.OnStartLocalPlayer();
        //     controllers.ForEach(controller => controller.AssignPlayers());
        // }

        /**
         * assign network player to mobile client
         */
        [TargetRpc]
        public void UpdateSceneConnected()
        {
            _controller.AssignPlayers();

            if (SceneManager.GetActiveScene().name == "AppOffline")
            {
                Debug.Log("update scene connected (MOBILE ONLY)");
                mobile = true;
                ((MobileController) _controller).AssignPlayer(this); // mozna presunout do onstrtlocalplayer
            }
        }

        public void ChangeScene(string oldScene, string newScene)
        {
            if (String.IsNullOrEmpty(newScene))
            {
                return;
            }
            
            DontDestroyOnLoad(this);
            string sceneToLoad = newScene;
            if (mobile)
            {
                Debug.Log("mobile true");
                sceneToLoad = "PlsMobile"; //TODO
            }

            _sceneLoader.LoadScene(sceneToLoad, true);
        }

        public void GoToLobby(bool oldValue, bool exit)
        {
            chosenWorld = String.Empty;
            if (mobile)
            {
                ((MobileController) _controller).OnGoToLobby();
            }
            else
            {
                _gameController = FindObjectOfType<GameController>();
                _gameController.GoToLobby();
            }
        }


        //mozna ze to coje ted v networkManager se da dat i sem jako serverova strana
        [Command(requiresAuthority = false)] //require authority
        public void CmdHandleSelectedWorld(string sceneName)
        {
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


        //volano serverem na clientech
        public void SetPlayerMoving(bool oldValue, bool moving)
        {
            Debug.Log("set player moving in Network pLayer, moving: " + moving);

            Debug.Log(mobile);
            if (!mobile)
            {
                AssignGameController(); //TODO nejak jinak mozna smazat

                if (moving)
                {
                    Debug.Log("VR plater moving true");
                    _gameController.StartMovement();
                }
                else
                {
                    _gameController.PauseMovement();
                }
            }
        }

        public void SkipCalibration(bool oldValue, bool skip)
        {
            Debug.Log("set player skip calibration CALLBACK in Network pLayer, skip: " + skip);
            if (!mobile)
            {
                ((VRLobbyController) _controller).GetCartCreator().SkipCalibration();
            }
        }

        private void AssignGameController()
        {
            _gameController = FindObjectOfType<GameController>();
            Debug.Log(_gameController);
        }

        //TODO
        public void ResumeCartDrive()
        {
            Debug.Log("cart resumes going");
        }

        private bool IsMobileClientConnected()
        {
            var players = FindObjectsOfType<NetworkPlayer>();
            if (players.Any(player => player.mobile))
            {
                Debug.Log("mobile network player");
                return true;
            }
            return false;
        }


        [Command(requiresAuthority = false)]
        public void CmdSetCalibrationComplete(bool complete)
        {
            Debug.Log("CMD set calibrationComplete in Network pLayer, complete: " + complete);
            calibrationComplete = complete;
        }

        //called automatically, after calibrationComplete changes
        private void SetCalibrationComplete(bool oldValue, bool complete)
        {
            Debug.Log("Set calibration complete callback nETWORK PLAYER");
            OnCalibrationComplete?.Invoke(); //ma si to prevzit mobile controller (MobileController)
        }
    }
}