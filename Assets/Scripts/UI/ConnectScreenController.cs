using Mirror;
using Mirror.Discovery;
using Network;
using TMPro;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ConnectScreenController : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI connectableDeviceText;
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
        }

        private void DisplayDiscoveredServers(ServerResponse serverResponse)
        {
            Debug.Log("connectable device found PICO");
            _serverResponse = serverResponse;
            connectableDeviceText.text = serverResponse.EndPoint.Address.ToString();
            availabilityStatus.SetSelected(true);
            hintBar.SetActive(false);
            ActivateConnectButton();
        }

        void Start()
        {
            myNetworkDiscovery.StartDiscovery();
        }

        private void Connect()
        {
            connectButton.GetComponent<Button>().interactable = false;
            if (_serverResponse.Equals(null))
            {
                Debug.Log("SERVER response null");
                return;
            }
            networkManager.StartClient(_serverResponse.uri);
        }

        private void IndicateConnectedStatus()
        {
            Debug.Log("server added player PLSPLSPS");
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
