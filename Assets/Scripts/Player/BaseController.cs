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
        protected bool _worldSelected = false;

        public virtual void OnDisconnect()
        {
            sceneLoader.UnloadScene(); //VR dobry?
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
            if (_worldSelected)
            {
                Debug.Log("sceneCount > 1");
                return;
            }
            _worldSelected = true;

            LocalNetworkPlayer.CmdHandleSelectedWorld(sceneName); //message about scene loading to other players
            // uiController.DisplayLoader(true); //TODO je to potreba?
        }
    }
}