
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;
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

        /**
         * assigns LocalNetworkPlayer and RemoteNetworkPlayer variables from all player objects in the scene 
         */
        public void AssignPlayers()
        {
            NetworkPlayers = FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in NetworkPlayers)
            {
                if (networkPlayer.isLocalPlayer)
                {
                    LocalNetworkPlayer = networkPlayer;
                    LocalNetworkPlayer.OnCalibrationComplete += OnCalibrationComplete;
                }
                else
                {
                    RemoteNetworkPlayer = networkPlayer;
                }
            }
        }

        protected virtual void OnCalibrationComplete() { }

        public virtual void SkipCalibration() { }

        /**
         * synchronises calibration complete syncVar with all NetworkPlayers in the scene
         */
        protected virtual void SetCalibrationComplete()
        {
            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }
            
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSetCalibrationComplete(true);
            }
        }

        /**
         * handles selecting a scene (if no world scene is loaded/loading)
         * and triggers network synchronisation with Command CmdHandleSelectedWorld on all NetworkPlayers in the scene
         */
        public virtual void OnSceneSelected(string sceneName)
        {
            if (SceneManager.sceneCount > 1)
            {
                Debug.Log("scene count > 1");
                return;
            }

            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdHandleSelectedWorld(sceneName);
            }
        }

        public virtual void OnGoToLobby() { }

        /**
         * synchronises triggerGoToLobby  syncVar with all NetworkPlayers in the scene
         */
        public virtual void TriggerGoToLobby()
        {
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdGoToLobby();
            }
        }

        protected virtual void OnSceneLoaded() { }

        /**
         * handles selected language with given index of the language
         */
        public void OnLanguageSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
        


    }
}