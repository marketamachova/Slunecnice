using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Network
{
    public class MyNetworkManager : NetworkManager
    {
        public GameObject PlayerCamera {get; private set;}
        public static List<GameObject> Players {get; private set;} = new List<GameObject>();
        public event Action OnServerAddPlayerAction;
        public event Action OnClientConnectAction;
        public event Action OnClientDisconnectAction;
        

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            InstantiatePlayer(conn);

            Debug.Log($"server added player. there are {numPlayers} connected.");

            if (numPlayers <= 1)
            {
                InstantiateCamera();
            }

            OnServerAddPlayerAction?.Invoke();
        }

        private void InstantiatePlayer(NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            if (networkPlayer.netId >= 3)
            {
                networkPlayer.mobile = true;
            }

            NetworkServer.AddPlayerForConnection(conn, player);
            DontDestroyOnLoad(player);
            Players.Add(player);
            Debug.Log("added a player, players length " + Players.Count);
            
            networkPlayer.UpdateSceneConnected();
        }

        private void InstantiateCamera()
        {
            Debug.Log("adding a camera");
            
            PlayerCamera = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "NetworkPlayerAttachCamera"));
            NetworkServer.Spawn(PlayerCamera);
            DontDestroyOnLoad(PlayerCamera);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            Debug.Log("client disconnected");
            base.OnClientDisconnect(conn);
            OnClientDisconnectAction?.Invoke();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            OnClientConnectAction?.Invoke();
        }
    }
}
