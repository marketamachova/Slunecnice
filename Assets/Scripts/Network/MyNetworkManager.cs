﻿using System;
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


        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            InstantiatePlayer(conn);

            Debug.Log($"server added player. there are {numPlayers} connected.");

            if (numPlayers <= 1)
            {
                InstantiateCamera();
            }
            else
            {
                OnMobileClientConnectAction?.Invoke();
            }

            Debug.Log("conn.netid" + conn.identity.netId);

            OnServerAddPlayerAction?.Invoke();
        }

        private void InstantiatePlayer(NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            if (numPlayers > 1) //TODO asi ne
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
            var mainCamera = GameObject.FindWithTag("MainCamera");
            mainCamera.transform.parent = PlayerCamera.transform;
            DontDestroyOnLoad(mainCamera);
            DontDestroyOnLoad(PlayerCamera);

        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            Debug.Log(numPlayers);
            Debug.Log(conn.connectionId);
            if (numPlayers > 0)
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
            Debug.Log("client disconnected");
            base.OnClientDisconnect(conn);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            OnClientConnectAction?.Invoke();
        }
    }
}