using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkController : NetworkBehaviour
{
    private void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Pressed Start");
            StartTravel();
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("Server has started");
    }

    [Command]
    private void StartTravel()
    {
        Debug.Log("Starting travel");
    }

}

