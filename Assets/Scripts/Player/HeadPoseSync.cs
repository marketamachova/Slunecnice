using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class HeadPoseSync : MonoBehaviour
{
    [SerializeField] private MyNetworkManager networkManager;

    private void Awake()
    {
        // networkManager.ServerAddedPlayer += SetPlayerAsParent;
    }

    void Start()
    {
        
    }
}
