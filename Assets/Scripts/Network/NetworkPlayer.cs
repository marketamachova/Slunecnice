using System;
using System.Collections.Generic;
using Mirror;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Network
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] public bool mobile = false;

        [SyncVar(hook = "ChangeScene")]
        public string chosenWorld;
        
        [SyncVar(hook = "SetPlayerMoving")]
        public bool playerMoving;
        
        [SyncVar(hook = "SkipCalibration")]
        public bool skipCalibration;
        
        [SyncVar(hook = "SetCalibrationComplete")]
        public bool calibrationComplete;
        
        private BaseUIController _uiController;
        private GameController _gameController;
        private Controller _mobileController;
        private VRLobbyController _vrLobbyController;

        public event Action OnCalibrationComplete;

        private void Awake()
        {
            _vrLobbyController = GameObject.FindObjectOfType<VRLobbyController>();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (mobile)
            {
                _mobileController.OnDisconnect();
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
            if (SceneManager.GetActiveScene().name == "AppOffline")
            {
                Debug.Log("update scene connected (MOBILE ONLY)");
                mobile = true;
                _mobileController = GameObject.FindObjectOfType<Controller>();
                _mobileController.AssignPlayer(this); // mozna presunout do onstrtlocalplayer
            }
        }

        public void ChangeScene(string oldScene, string newScene)
        {
            DontDestroyOnLoad(this);
            var currentScene = SceneManager.GetActiveScene().name;
            Debug.Log(currentScene);
            if (currentScene == "VROffline") //change scene for VR from mobile
            {
                Debug.Log("about to change scene in VR");

                VRLobbyController vrLobbyController =
                    GameObject.Find("LobbyController").GetComponent<VRLobbyController>();
                vrLobbyController.OnSceneSelected(chosenWorld);
            }
        }
        

        //mozna ze to coje ted v networkManager se da dat i sem jako serverova strana
        [Command]
        public void CmdHandleSelectedWorld(string sceneName)
        {
            chosenWorld = sceneName; //changing syncvar as cmd results in server synchronising all clients
        }

        //volano Controllerem
        [Command]
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
            if (!mobile)
            {
                AssignGameController();

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
                _vrLobbyController.GetCartCreator().SkipCalibration();
            }
        }

        private void AssignGameController()
        {
            _gameController = GameObject.FindWithTag("Controller").GetComponent<GameController>();
            Debug.Log(_gameController);
            Debug.Log(SceneManager.GetActiveScene().name);
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

        // [Command]
        // public void CmdSetCalibrationComplete(bool completed)
        // {
        //     Debug.Log("CMD set calibration completed in Network pLayer, calibration completed: " + completed);
        //     calibrationComplete = completed;
        // }
        
        
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
            OnCalibrationComplete?.Invoke(); //ma si to prevzit mobile controller (Controller)
            if (mobile)
            {
                Debug.Log("mobile true");
                _mobileController.OnCalibrationComplete();
            }
        }
    }
}