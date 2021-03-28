using System;
using System.Collections;
using Cart;
using Mirror;
using Mirror.Discovery;
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
        [SerializeField] private NetworkDiscovery networkDiscovery;
        [SerializeField] private UIControllerVRLobby uiController;
        [SerializeField] private CartCreator cartCreator;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private GameObject floor;
        
        //debug only
        private bool _spacePressed = false;

        private void Awake()
        {
            cartCreator.OnCartCreated += EnableSceneSelection;
        }

        IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
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
        
        private void EnableSceneSelection()
        {
            Debug.Log("enable scene selection");
            uiController.EnablePanelExclusive("SceneSelection");
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