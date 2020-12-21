using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlApp : MonoBehaviour
{
    [SerializeField] Button joinButton;

    public void Join()
    {
        //check if join was sucessful
        joinButton.interactable = false;

        Client.localClient.JoinGame();
    }
}
