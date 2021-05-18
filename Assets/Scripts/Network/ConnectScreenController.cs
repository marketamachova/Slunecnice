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
    
        private ServerResponse _serverResponse;

        void Awake()
        {
            myNetworkDiscovery.OnServerFound.AddListener(DisplayDiscoveredServers);
            networkManager.OnClientConnectAction += IndicateConnectedStatus;
            networkManager.OnMobileClientDisconnectAction += OnDisconnect;
        }

        private void DisplayDiscoveredServers(ServerResponse serverResponse)
        {
            Debug.Log("connectable device found");
            _serverResponse = serverResponse;
            deviceName.text = serverResponse.EndPoint.Address.ToString();
            deviceName.gameObject.SetActive(true);
            deviceNotFound.SetActive(false);
            availabilityStatus.SetSelected(true);
            hintBar.SetActive(false);
            ActivateConnectButton();
        }

        void Start()
        {
            myNetworkDiscovery.StartDiscovery();
        }

        public void OnDisconnect()
        {
            Debug.Log("ON DISCONNECT CONNECT screen controller");   
            connectedText.SetActive(false);
            connectButton.SetActive(true);
            connectButton.GetComponent<Button>().interactable = false;
            availabilityStatus.SetSelected(false);
            deviceName.gameObject.SetActive(false);
            deviceNotFound.SetActive(true);
            hintBar.SetActive(true);
            myNetworkDiscovery.StartDiscovery();
        }

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
        
        private void ActivateConnectButton()
        {
            var button = connectButton.GetComponent<Button>();
            button.onClick.AddListener(Connect);
            button.interactable = true;
        }
    }
}
