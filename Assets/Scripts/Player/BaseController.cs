using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;
using Mirror;
using Mirror.Discovery;
using Network;
using Scenes;
using UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace Player
{
    public class BaseController : MonoBehaviour
    {
        [SerializeField] protected NetworkDiscovery networkDiscovery;
        [SerializeField] protected MyNetworkManager networkManager;
        [SerializeField] protected SceneLoader sceneLoader;
        [SerializeField] protected BaseUIController uiController;
        
        protected NetworkPlayer[] NetworkPlayers;
        protected NetworkPlayer LocalNetworkPlayer;
        protected NetworkPlayer RemoteNetworkPlayer;

        public void Awake()
        {
            networkManager.OnServerAddPlayerAction += AssignPlayers;
        }

        public virtual void OnDisconnect()
        {
            if (SceneManager.sceneCount > 1)
            {
                sceneLoader.UnloadScene();
            }
            uiController.DisplayError();
        }

        public virtual void AssignPlayers()
        {
            Debug.Log("assign players in BASE Controller called");
            NetworkPlayers = GameObject.FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in NetworkPlayers)
            {
                if (networkPlayer.isLocalPlayer)
                {
                    Debug.Log("found network player LOCAL");
                    
                    LocalNetworkPlayer = networkPlayer;
                    LocalNetworkPlayer.OnCalibrationComplete += OnCalibrationComplete;
                }
                else
                {
                    RemoteNetworkPlayer = networkPlayer;
                }
            }
        }

        public virtual void OnCalibrationComplete()
        {
        }

        public virtual void SkipCalibration()
        {
        }

        public virtual void SetCalibrationComplete()
        {
            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }
            
            foreach (var networkPlayer in NetworkPlayers)
            {
                Debug.Log("setting cmdsetcalibrationcomplete");
                networkPlayer.CmdSetCalibrationComplete(true);
            }
        }

        public virtual void OnSceneSelected(string sceneName)
        {
            Debug.Log("OnSceneSelected .......");
            if (SceneManager.sceneCount > 1)
            {
                Debug.Log("sceneCount > 1");
                return;
            }

            foreach (var networkPlayer in NetworkPlayers)
            {
                Debug.Log("setting cmd hanlde selected world called");
                // Debug.Log(sceneName);
                networkPlayer.CmdHandleSelectedWorld(sceneName);
            }//message about scene loading to other players
        }

        public virtual void OnGoToLobby()
        {
        }

        public virtual void TriggerGoToLobby()
        {
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdGoToLobby();
            }
        }

        public virtual void OnSceneLoaded()
        {
            
        }

        public void OnLanguageSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
        


    }
}