using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using NetworkPlayer = Network.NetworkPlayer;

namespace UI
{
    enum WatchMode {PlayerCamera, TopCamera, Dashboard}

    public class UIController : BaseUIController
    {
        [SerializeField] public bool portraitOriented;
        
        [SerializeField] private List<GameObject> screenPanels;
        [SerializeField] private List<SelectableButton> buttons;
        [SerializeField] private List<GameObject> cameraViews;
        [SerializeField] private List<GameObject> controls;
        [SerializeField] private GameObject sceneSelectionPanel;
        [SerializeField] private string ipAddress;
        [SerializeField] private WatchMode watchMode;

        private NetworkPlayer _networkPlayer;
        private SceneLoader _sceneLoader;
        private bool _controlsVisible;

        private void Awake()
        {
            networkManager.OnClientDisconnectAction += DisplayError;
            _sceneLoader = GetComponent<SceneLoader>();
        }

        //general controller
        public void Join()
        {
            networkManager.networkAddress = ipAddress;
            networkManager.StartClient();
        }

        public void EnablePanel(string panelName)
        {
            screenPanels.ForEach(panel => panel.SetActive(panel.name == panelName));
        }

        public void EnableCameraView(string cameraViewName)
        {
            cameraViews.ForEach(panel => panel.SetActive(panel.name == cameraViewName));
        }

        public void ActivateButton(string buttonName)
        {
            buttons.ForEach(button => button.SetSelected(button.name == buttonName));
        }
        
        public void OnChangeScreenOrientation()
        {
            Screen.orientation = portraitOriented ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait;
            portraitOriented = !portraitOriented;
        }
        
        //wtf co je tohle
        public void ToggleControlsVisible()
        {
            _controlsVisible = !_controlsVisible;
            if (_controlsVisible)
            {
                //set active only if current panel is their panel OR move them
                controls.ForEach(control => control.SetActive(true));
                StartCoroutine(HideControls());
            }
        }

        public void SetControlsVisible()
        {
            _controlsVisible = true;
            controls.ForEach(control => control.SetActive(true));
        }

        private IEnumerator HideControls()
        {
            while (_controlsVisible)
            {
                yield return new WaitForSecondsRealtime(5);
                controls.ForEach(control => control.SetActive(false));
            }
        }

        //general controller
        public void OnSceneChosen(string chosenScene)
        {
            _networkPlayer.CmdHandleSelectedWorld(chosenScene); //message about scene loading to other players
            _sceneLoader.LoadScene(chosenScene, true);
        }


        //general controller
        public void AssignPlayer(NetworkPlayer player)
        {
            Debug.Log("assigning player ");
            _networkPlayer = player;
            Debug.Log(_networkPlayer);
            sceneSelectionPanel.SetActive(true);
        }
    }
}