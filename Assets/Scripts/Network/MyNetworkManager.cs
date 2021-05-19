using System;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

namespace Network
{
    public class MyNetworkManager : NetworkManager
    {
        public GameObject PlayerCamera { get; private set; }
        public static List<GameObject> Players { get; private set; } = new List<GameObject>();
        public event Action OnServerAddPlayerAction;
        public event Action OnClientConnectAction;
        public event Action OnClientDisconnectAction;
        public event Action OnMobileClientConnectAction;
        public event Action OnMobileClientDisconnectAction;


        /**
         * callback called automatically after server added player
         * 1. instantiates player object
         * 2. instantiate object carrying player and cameras
         * 3. invokes established connection events
         */
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            InstantiatePlayer(conn);
            
            if (numPlayers <= 1)
            {
                InstantiateCamera();
            }
            else
            {
                OnMobileClientConnectAction?.Invoke();
            }
            
            OnServerAddPlayerAction?.Invoke();
        }

        /**
         * instantiates player with given NetworkConnection
         * adds player to Players list
         * calls UpdateSceneConnected on the instantiated player to update UI
         */
        private void InstantiatePlayer(NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();

            NetworkServer.AddPlayerForConnection(conn, player);
            DontDestroyOnLoad(player);
            Players.Add(player);

            networkPlayer.UpdateSceneConnected();
        }

        /**
         * instantiate gameObject carrying all cameras
         */
        private void InstantiateCamera()
        {
            PlayerCamera = Instantiate(spawnPrefabs.Find(prefab => prefab.name == GameConstants.NetworkPlayerAttachCamera));
            NetworkServer.Spawn(PlayerCamera);

            var mainCamera = GameObject.FindWithTag(GameConstants.MainCamera);
            mainCamera.transform.parent = PlayerCamera.transform;
            
            DontDestroyOnLoad(PlayerCamera);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            if (numPlayers >= 1)
            {
                OnMobileClientDisconnectAction?.Invoke();
            }
            else
            {
                OnClientDisconnectAction?.Invoke();
            }
        }


        public override void OnClientDisconnect(NetworkConnection conn)
        {
            OnClientDisconnectAction?.Invoke();
            base.OnClientDisconnect(conn);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            OnClientConnectAction?.Invoke();
        }
    }
}