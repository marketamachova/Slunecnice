using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] protected List<Selectable> activables;
        [SerializeField] protected List<GameObject> enalables;
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

        public void EnableTrue(string enalableName)
        {
            foreach (var enalable in enalables.Where(enalable => enalable.name == enalableName))
            {
                enalable.SetActive(true);
            }
        }
        
        public void EnableFalse(string enalableName)
        {
            foreach (var enalable in enalables.Where(enalable => enalable.name == enalableName))
            {
                enalable.SetActive(false);
            }
        }
        
        public void ActivateExclusive(string activableName)
        {
            activables.ForEach(activable => activable.SetSelected(activable.name == activableName));
        }

        public void Activate(string activableName)
        {
            foreach (var selectable in activables.Where(selectable => selectable.name == activableName))
            {
                selectable.SetSelected(true);
            }
        }
        
        public void Deactivate(string activableName)
        {
            foreach (var selectable in activables.Where(selectable => selectable.name == activableName))
            {
                selectable.SetSelected(false);
            }
        }
    }
}
