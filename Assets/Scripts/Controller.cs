using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetworkPlayer = Network.NetworkPlayer;

public class Controller : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private MyNetworkManager networkManager;
    private GameObject[] _players = new GameObject[4];
    private NetworkPlayer _networkPlayer;

    public void Awake()
    {
        sceneLoader.SceneLoadingEnd += OnSceneLoaded;
        networkManager.OnServerAddPlayerAction += AssignPlayers;
    }
    
    private void AssignPlayers()
    {
        Debug.Log("assign players in Controller called !!!!");
        _players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void OnSceneLoaded()
    {
        Debug.Log("Controller scene loaded");
        uiController.EnablePanel("WatchScreenPortrait");
        uiController.SetControlsVisible();
        uiController.ActivateButton("PlayerCameraButton");
    }
    
    

    public void StartDrive()
    {
        if (_players.Length == 0)
        {
            AssignPlayers();
        }
        
        foreach (var player in _players)
        {
            Debug.Log(player);
            var networkPlayer = player.GetComponent<NetworkPlayer>();
            if (networkPlayer.isLocalPlayer)
            {
                _networkPlayer.CmdSetPlayerMoving(true);
            }
        }
    }

    public void PauseDrive()
    {
        Debug.Log("VRController pause drive");
        _networkPlayer.CmdSetPlayerMoving(false);

    }

    public void ResumeDrive()
    {
        Debug.Log("VRController resume drive");
        _networkPlayer.CmdSetPlayerMoving(true);
    }

    public void EndDrive()
    {
        Debug.Log("VRController end drive");
        _networkPlayer.CmdSetPlayerMoving(false); //indicate END somehow
    }
}
