using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar] public Transform cameraRotation;
    private GameController _gameController;

    public void Start()
    {
        _gameController = GetComponent<GameController>();
    }

    [Command]
    public void StopCart()
    {
        Debug.Log("cart stopped");
        _gameController.End(); //should add pause later
    }
    
    [Command] //TODO
    public void ResumeCartDrive()
    {
        Debug.Log("cart resumes going");
    }
}
