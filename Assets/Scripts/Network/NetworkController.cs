using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;


public class NetworkController : NetworkBehaviour
{

    public override void OnStartServer()
    {
        Debug.Log("Server has started");
    }

    [Command]
    public void StartTravel()
    {
        Debug.Log("Starting travel");
        //Ahoj();
        SceneManager.LoadScene("HandsScene", LoadSceneMode.Single);
    }

    [Client]
    private void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Calling Ahoj");
            Ahoj();
        }
    }

    [Client]
    public void StartTravelTest()
    {
        Debug.Log("TEST Starting travel");
        if (isLocalPlayer)
        {
            Debug.Log("IsLocalPlayer");
            Ahoj();
        } else
        {
            Debug.Log("IsNot LocalPlayer");
            Ahoj();
        }
    }

    [Command]
    public void Ahoj()
    {
        Debug.Log("Ahoj called");
        //SceneManager.LoadScene("HandsScene", LoadSceneMode.Single);
       
    }



}

