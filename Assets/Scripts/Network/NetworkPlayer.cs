using System;
using Mirror;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] public bool mobile = false;

        [SyncVar(hook = "ChangeScene")] public string chosenWorld;

        [SyncVar(hook = "SetPlayerMoving")] public bool playerMoving = true;

        [SyncVar(hook = "SkipCalibration")] public bool skipCalibration;

        [SyncVar(hook = "SetCalibrationComplete")]
        public bool calibrationComplete;

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

        // //TODO je to k necemu?
        public void ChangeScene(string oldScene, string newScene)
        {
            DontDestroyOnLoad(this);
            _sceneLoader.LoadScene(newScene, true);

        }


        //mozna ze to coje ted v networkManager se da dat i sem jako serverova strana
        [Command] //require authority
        public void CmdHandleSelectedWorld(string sceneName)
        {
            chosenWorld = sceneName; //changing syncvar as cmd results in server synchronising all clients
        }

        //volano Controllerem
        [Command] //require authority
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


        //volano serverem na clientech
        public void SetPlayerMoving(bool oldValue, bool moving)
        {
            Debug.Log("set player moving in Network pLayer, moving: " + moving);

            Debug.Log(mobile);
            // if (!mobile)
            {
                AssignGameController(); //TODO nejak jinak mozna smazat

                if (moving)
                {
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


        [Command] //require authority
        public void StopCart()
        {
            Debug.Log("cart stopped");
            _gameController.End(); //should add pause later
        }

        //TODO
        public void ResumeCartDrive()
        {
            Debug.Log("cart resumes going");
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