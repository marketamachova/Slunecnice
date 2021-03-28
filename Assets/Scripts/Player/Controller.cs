using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cart;
using Network;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using NetworkPlayer = Network.NetworkPlayer;

public class Controller : MonoBehaviour
{
    [FormerlySerializedAs("uiController")] [SerializeField] private UIControllerMobile uiControllerMobile;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private MyNetworkManager networkManager;
    private List<GameObject> _players = new List<GameObject>();
    private NetworkPlayer _networkPlayer;
    private bool _playing = false;

    public void Awake()
    {
        sceneLoader.SceneLoadingEnd += OnSceneLoaded;
        networkManager.OnServerAddPlayerAction += AssignPlayers;
    }

    private void AssignPlayers()
    {
        Debug.Log("assign players in Controller called");
        var playersArr = GameObject.FindGameObjectsWithTag("Player");
        _players.AddRange(playersArr);
        Debug.Log(_players.Count);
        foreach (var player in _players)
        {
            Debug.Log(player);
        }
    }

    private void OnSceneLoaded()
    {
        Debug.Log("Controller scene loaded");
        uiControllerMobile.EnablePanelExclusive("WatchScreenPortrait");
        uiControllerMobile.ToggleControlsVisible();
        uiControllerMobile.ActivateButton("PlayerCameraButton");
    }

    public void OnPlayPressed()
    {
        _playing = !_playing;
        
        if (_players.Count == 0)
        {
            AssignPlayers();
        }

        foreach (var networkPlayer in _players.Select(player => player.GetComponent<NetworkPlayer>()))
        {
            Debug.Log(networkPlayer);
            if (networkPlayer.hasAuthority)
            {
                networkPlayer.CmdSetPlayerMoving(_playing);
            }
        }
        
        uiControllerMobile.OnPlayPressed(_playing);
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