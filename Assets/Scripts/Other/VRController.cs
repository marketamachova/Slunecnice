using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class VRController : MonoBehaviour
// {
//     private GameObject _player;
//     private NetworkPlayer _networkPlayer;
//
//     public void Start()
//     {
//         _player = GameObject.FindWithTag("Player");
//         if (_player == null)
//         {
//             Debug.Log("player null");
//             return;
//         }
//         _networkPlayer = _player.GetComponent<NetworkPlayer>();
//     }
//
//     public void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             _player = GameObject.FindWithTag("Player");
//             if (_player == null)
//             {
//                 Debug.Log("player null22");
//                 return;
//             }
//             _networkPlayer = _player.GetComponent<NetworkPlayer>();
//             Debug.Log("space pressed, pausing drive");
//             PauseDrive();
//         }
//     }
//
//     public void PauseDrive()
//     {
//         Debug.Log("VRController pause drive");
//         _networkPlayer.StopCart();
//     }
//     
//     public void ResumeDrive()
//     {
//         Debug.Log("VRController resume drive");
//         _networkPlayer.ResumeCartDrive();
//     }
//     
//     public void EndDrive()
//     {
//         Debug.Log("VRController end drive");
//         _networkPlayer.StopCart();
//     }
// }
