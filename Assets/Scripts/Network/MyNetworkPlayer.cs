using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MyNetworkPlayer : NetworkBehaviour
{
    public GameObject startButton;

    public void Start()
    {
        
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        
        Debug.Log("On start authority");
        
        var buttonComp = startButton.GetComponent<Button>();
        buttonComp.onClick.AddListener(HandleStart);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleStart();
        }
    }

    [SyncVar] public string displayName = "Name";

    [Server]
    public void SetDisplayName(string newName)
    {
        this.displayName = newName;
    }

    [Command]
    private void CmdStartVR()
    {
        Debug.Log("Start VR Run on Server");
    }
    
    [ClientRpc]
    private void RpcStartVR()
    {
        Debug.Log("Start VR Run on Client");
    }

    public void HandleStart()
    {
        if (!hasAuthority)
        {
            Debug.Log("Does not have authority");

        }
        if (!isLocalPlayer)
        {
            Debug.Log("is not local Player");
        }
        CmdStartVR();
        RpcStartVR();
    }
}
