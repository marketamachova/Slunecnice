using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;
using Mirror;
using Mirror.Discovery;
using Network;
using Scenes;
using UI;
using UnityEngine.SceneManagement;

namespace Player
{
    public class BaseController : MonoBehaviour
    {
        [SerializeField] protected NetworkDiscovery networkDiscovery;
        [SerializeField] protected MyNetworkManager networkManager;
        [SerializeField] protected SceneLoader sceneLoader;
        [SerializeField] protected BaseUIController uiController;

        protected readonly List<NetworkPlayer> Players = new List<NetworkPlayer>();
        protected NetworkPlayer LocalNetworkPlayer;
        protected bool WorldSelected = false;
        protected string CurrentScene;

        public virtual void OnDisconnect()
        {
            if (SceneManager.sceneCount > 1)
            {
                sceneLoader.UnloadScene(); //VR dobry?
            }
            uiController.DisplayError();
        }

        public virtual void AssignPlayers()
        {
            Debug.Log("assign players in BASE Controller called");
            var networkPlayers = GameObject.FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in networkPlayers)
            {
                Players.Add(networkPlayer);
                if (networkPlayer.isLocalPlayer)
                {
                    Debug.Log("found network player LOCAL");
                    LocalNetworkPlayer = networkPlayer;
                    LocalNetworkPlayer.OnCalibrationComplete += OnCalibrationComplete;
                }
            }
        }

        public virtual void OnCalibrationComplete()
        {
            Debug.Log("base onCalibrationComplete");
        }

        public virtual void SetCalibrationComplete()
        {
            var players = GameObject.FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in players)
            {
                networkPlayer.CmdSetCalibrationComplete(true);
            }

            Debug.Log("SET calibration complete Base controller");
        }

        public virtual void OnSceneSelected(string sceneName)
        {
            Debug.Log("OnSceneSelected .......");
            if (WorldSelected)
            {
                Debug.Log("sceneCount > 1");
                return;
            }

            Debug.Log(LocalNetworkPlayer);
            LocalNetworkPlayer.CmdHandleSelectedWorld(sceneName); //message about scene loading to other players
            WorldSelected = true;
        }

        public virtual void OnGoToLobby()
        {
        }

        public virtual void TriggerGoToLobby()
        {
            WorldSelected = false;
            var players = GameObject.FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in players)
            {
                networkPlayer.CmdGoToLobby();
            }
        }
    }
}