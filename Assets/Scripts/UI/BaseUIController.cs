using Network;
using UnityEngine;

namespace UI
{
    public class BaseUIController : MonoBehaviour
    {
        [SerializeField] private GameObject networkStatusIndicator;
        [SerializeField] private GameObject networkStatusIndicatorMobile;
        [SerializeField] private GameObject loadingSceneIndicator;
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
    }
}
