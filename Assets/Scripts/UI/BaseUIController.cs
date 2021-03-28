using System.Collections.Generic;
using Network;
using UnityEngine;

namespace UI
{
    public class BaseUIController : MonoBehaviour
    {
        [SerializeField] private GameObject networkStatusIndicator;
        [SerializeField] private GameObject networkStatusIndicatorMobile;
        [SerializeField] private GameObject loadingSceneIndicator;
        [SerializeField] protected List<GameObject> panels;
        [SerializeField] protected List<GameObject> activables;
        [SerializeField] protected MyNetworkManager networkManager;
        
        [SerializeField] private GameObject errorPanel;

        private int _numPlayers = 0;
        
        private void Start()
        {
            networkManager.OnServerAddPlayerAction += DisplayPlayersConnected;
        }

        private void DisplayPlayersConnected()
        {
            _numPlayers++;
            networkStatusIndicator.SetActive(true);
            networkStatusIndicatorMobile.SetActive(_numPlayers > 1);
            
        }

        public void DisplayLoader()
        {
            loadingSceneIndicator.SetActive(true);
        }

        public virtual void DisplayError()
        {
            errorPanel.SetActive(true);
        }
        
        public void EnablePanelExclusive(string panelName)
        {
            panels.ForEach(panel => panel.SetActive(panel.name == panelName));
        }

        public void ActivateTrue(string activableName)
        {
            activables.ForEach(activable =>
            {
                if (activable.name == activableName)
                {
                    activable.SetActive(true);
                }
            });
        }
        
        public void ActivateFalse(string activableName)
        {
            activables.ForEach(activable =>
            {
                if (activable.name == activableName)
                {
                    activable.SetActive(false);
                }
            });
        }
    }
}
