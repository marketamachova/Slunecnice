using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement _playerMovement;
    
    void Start()
    {
        _playerMovement = player.GetComponent<PlayerMovement>();

        StartCoroutine(Wait());
    }
    
    
    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(5);
        _playerMovement.enabled = true;
    }
}
