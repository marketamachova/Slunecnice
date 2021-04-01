using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;
using Mirror;

namespace Player
{
    public class BaseController : MonoBehaviour
    {
        protected readonly List<NetworkPlayer> Players = new List<NetworkPlayer>();
        protected NetworkPlayer LocalNetworkPlayer;

        public virtual void AssignPlayers()
        {
            Debug.Log("assign players in BASEController called");
            var playersArr = GameObject.FindGameObjectsWithTag("Player");
            foreach (var networkPlayer in playersArr.Select(player => player.GetComponent<NetworkPlayer>()))
            {
                Debug.Log("NETWORK PLAYER dfkjhsdfkjhsdfkjsh");
                Players.Add(networkPlayer);
                if (networkPlayer.isLocalPlayer)
                {
                    Debug.Log("found network player LOCAL");
                    LocalNetworkPlayer = networkPlayer;
                }
            }
        }

        protected virtual void OnCalibrationComplete()
        {
            Debug.Log("On calibration complete Base controller");
        }
    }
}