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
        [SerializeField] public bool mobile;
        [SerializeField] private MyNetworkManager networkManager;
        [SerializeField] private string ipAddress;

        [SyncVar(hook = "ChangeScene")]
        public string chosenWorld;
        
        [SyncVar(hook = "SetPlayerMoving")]
        public bool playerMoving;
        
        private BaseUIController _uiController;
        private GameObject _uiControllerGO;
        private GameController _gameController;

        public void Start()
        {
            // _gameController = GameObject.FindWithTag("Controller").GetComponent<GameController>();
            _uiControllerGO = GameObject.FindWithTag("UIController");
            
        }

        /**
         * display available scenes in mobile and VR
         */
        [TargetRpc]
        public void UpdateSceneConnected()
        {
            if (SceneManager.GetActiveScene().name == "AppOffline")
            {
                Debug.Log("update scene connected (MOBILE ONLY)");
                _uiControllerGO = GameObject.FindWithTag("UIController");
                Debug.Log(_uiControllerGO);
                var uiController = _uiControllerGO.GetComponent<UIController>();
                
                uiController.AssignPlayer(this);
            }
        }

        public void ChangeScene(string oldScene, string newScene)
        {
            Debug.Log("LOADCHOSENSCENE DOPICI");
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

        private void AssignGameController()
        {
            _gameController = GameObject.FindWithTag("Controller").GetComponent<GameController>();
            Debug.Log(_gameController);
            Debug.Log(SceneManager.GetActiveScene().name);
        }

        private void HandleClientDisconnect()
        {
            Debug.Log("client disconnected err");
        }


        // [Command(ignoreAuthority = true)]
        // public void StartCart()
        // {
        //     _mainCamera = GameObject.FindWithTag("MainCamera");
        //     _gameController = _mainCamera.GetComponent<GameController>();
        //     _gameController.StartMovement(); //should add pause later
        // }

        [Command(ignoreAuthority = true)]
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
    }
}