using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private Quaternion startingRotation;
    [SerializeField] private MyNetworkManager networkManager;
    private GameObject _playerCamera;

    void Start()
    {
        _playerCamera = GameObject.FindWithTag("NetworkCamera");
        Debug.Log(_playerCamera);
        MovePlayersAtStartingPosition();
    }

    private void MovePlayersAtStartingPosition()
    {
        _playerCamera.transform.position = startingPosition;
        _playerCamera.transform.rotation = startingRotation;
    }
    
    
}
