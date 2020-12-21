using UnityEngine;
using Mirror;
using System;

public class NetworkManagerApp : NetworkManager
{
    [Scene] [SerializeField] private string mainMenuScene = "";

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public override void OnStartServer()
    {
        Debug.Log("Starting sever");
    }

    public override void OnStartClient()
    {
        Debug.Log("Starting client");
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        base.OnClientDisconnect(connection);
        OnClientDisconnected?.Invoke(); // invoke event
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);
        OnClientConnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);

      
    }


    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        base.OnServerAddPlayer(connection);
    }

}
