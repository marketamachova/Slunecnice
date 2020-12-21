using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Client : NetworkBehaviour
{
    public static Client localClient;

    private void Start()
    {
        if (isLocalPlayer)
        {
            localClient = this;
        }
    }

    public void JoinGame()
    {

    }
}
