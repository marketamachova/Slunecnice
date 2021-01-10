using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerConnectionManager : MonoBehaviour
{
    [SerializeField] private NetworkManagerApp networkManager;

    private void Start()
    {
        networkManager.StartHost();
    }
}
