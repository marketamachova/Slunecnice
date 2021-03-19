using System.Collections;
using Mirror;
using Network;
using UI;
using UnityEngine;
using UnityEngine.UI;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    public enum World
    {
        MainScene,
        Forest,
        Winter,
        Spring
    }

    public class VRLobbyController : NetworkBehaviour
    {
        [SerializeField] private MyNetworkManager networkManager;
        [SerializeField] private LobbyUIController uiController;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private GameObject floor;

        [SerializeField] private GameObject text;
        
        //debug only
        private bool _spacePressed = false;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            networkManager.StartHost();
        }

        public void Update()
        {
            //debug only
            if (Input.GetKey(KeyCode.Space) && !_spacePressed)
            {
                _spacePressed = true;
                OnSceneSelected(World.MainScene.ToString());
            }
        }

        public void OnSceneSelected(string scene)
        {
            uiController.DisplayLoader();
            sceneLoader.LoadScene(scene, false);

            // text.GetComponent<Text>().text = "Players count: " + networkManager.Players.Count;
            // networkManager.Players.ForEach(player 
            //     => player.GetComponent<NetworkPlayer>().LoadChosenScene(networkManager.Players, scene));
        }
    }
}