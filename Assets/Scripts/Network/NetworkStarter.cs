using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkManager))]
public class NetworkStarter : MonoBehaviour
{

    NetworkManager manager;


    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        Debug.Log("now server should start hosting");
        manager.StartHost();
    }
}
