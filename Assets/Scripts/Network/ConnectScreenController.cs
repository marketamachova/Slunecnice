using System.Collections;
using Mirror.Discovery;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ConnectScreenController : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI deviceName;
        [SerializeField] private GameObject deviceNotFound;
        [SerializeField] private GameObject connectedText;
        [SerializeField] private GameObject connectButton;
        [SerializeField] private Selectable availabilityStatus;
        [SerializeField] private NetworkDiscovery myNetworkDiscovery;
        [SerializeField] private MyNetworkManager networkManager;
        [SerializeField] private GameObject hintBar;

        private Button _connectButtonComponent;

    
        private ServerResponse _serverResponse;

        void Awake()
        {
            myNetworkDiscovery.OnServerFound.AddListener(DisplayDiscoveredServers);
            networkManager.OnClientConnectAction += IndicateConnectedStatus;
            networkManager.OnMobileClientDisconnectAction += OnDisconnect;
            
            _connectButtonComponent = connectButton.GetComponent<Button>();
        }

        
        private void DisplayDiscoveredServers(ServerResponse serverResponse)
        {
            _serverResponse = serverResponse;
            deviceName.text = serverResponse.EndPoint.Address.ToString();
            
            StartCoroutine(UpdateConnectUI(true));
        }

        void Start()
        {
            myNetworkDiscovery.StartDiscovery();
        }

        public void OnDisconnect()
        {
            UpdateConnectUI(false);
            
            connectedText.SetActive(false);
            connectButton.SetActive(true);

            myNetworkDiscovery.StartDiscovery();
        }
        
        /**
         * called on press Join button
         * - sets the Join button to disabled
         * - tries to connect to the found server
         * - handles disconnection if the connection could not be established
         */
        private void Connect()
        {
            connectButton.GetComponent<Button>().interactable = false;

            try
            {
                networkManager.StartClient(_serverResponse.uri);
                myNetworkDiscovery.StopDiscovery();
            }
            catch
            {
                Debug.Log("could not connect to server");
                OnDisconnect();
            }
        }

     
        private void IndicateConnectedStatus()
        {
            connectedText.SetActive(true);
            connectButton.SetActive(false);
        }
        
        private void ActivateConnectButton(bool activate)
        {
            if (activate)
            {
                _connectButtonComponent.onClick.AddListener(Connect);
            }
            else
            {
                _connectButtonComponent.onClick.RemoveAllListeners();
            }
            _connectButtonComponent.interactable = activate;
        }

        private IEnumerator UpdateConnectUI(bool serverAvailable)
        {
            availabilityStatus.SetSelected(serverAvailable);
            deviceName.gameObject.SetActive(serverAvailable);
            deviceNotFound.SetActive(!serverAvailable);
            hintBar.SetActive(!serverAvailable);
            
            yield return new WaitForSecondsRealtime(1);
            ActivateConnectButton(serverAvailable);
        }
    }
}
