using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public enum AppState
    {
        SearchingForDevices, ReturningToLobby, None
    }

    public class LoadingStateHandler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> stateIndicatorPanels; 
        private AppState _appState;

        public AppState AppState
        {
            get => _appState;
            set => SetAppState(value);
        }

        public void SetAppState(AppState newState)
        {
            _appState = newState;
            if (newState == AppState.None)
            {
                HideAllPanels();
                return;
            }
            
            EnableExclusive(newState.ToString());
        }
        
        public void EnableExclusive(string enalableName)
        {
            stateIndicatorPanels.ForEach(panel => panel.SetActive(panel.name == enalableName));
        }

        public void HideAllPanels()
        {
            stateIndicatorPanels.ForEach(panel => panel.SetActive(false));
        }
    }
}

