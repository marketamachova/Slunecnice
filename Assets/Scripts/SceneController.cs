using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Transform startingPoint;
    [SerializeField] private Vector3 cameraStartingPosition;
    [SerializeField] private Quaternion cameraStartingRotation;
    [SerializeField] private Vector3 startingPositionLobby;
    [SerializeField] private Quaternion startingRotationLobby;
    [SerializeField] private MyNetworkManager networkManager;
    [SerializeField] private bool debug;
    private GameObject _player;
    private GameObject _mainCamera;

    void Start()
    {
        _player = GameObject.FindWithTag("NetworkCamera");
        _mainCamera = GameObject.FindWithTag("MainCamera");
        MovePlayersAtStartingPosition();
    }

    public void MovePlayersAtStartingPosition()
    {
        // if (_player == null)
        // {
        //     _player = GameObject.FindWithTag("NetworkCamera");
        //     _mainCamera = GameObject.FindWithTag("MainCamera");
        // }
        Debug.Log("MOVE player at starting position");
        _player.transform.position = startingPoint.position;
        _player.transform.rotation = startingPoint.rotation;
        _mainCamera.transform.parent = _player.transform;
        _mainCamera.transform.position = _player.transform.position;
        _mainCamera.transform.localPosition = new Vector3(0, 1.5f, -0.273f);
        Debug.Log(_mainCamera.transform.position);
        Debug.Log(_mainCamera.transform.localPosition);
        // _mainCamera.transform.Rotate(0, -90f, 0);
    }

    public void MovePlayersAtStartingPositionLobby()
    {
        _player.transform.position = startingPositionLobby;
        _player.transform.rotation = startingRotationLobby;
    }
}