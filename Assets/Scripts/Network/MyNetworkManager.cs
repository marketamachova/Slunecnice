using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer playerScript = conn.identity.GetComponent<MyNetworkPlayer>();
        playerScript.SetDisplayName($"Player{numPlayers}");
        
        Debug.Log("Server added player"); //log to sever
        Debug.Log($"There are {numPlayers} players on the server"); 

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Client connected"); //log to sever
    }
    
}
