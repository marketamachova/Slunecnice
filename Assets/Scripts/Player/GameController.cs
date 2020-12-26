using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject countDown;
    private PlayerMovement _playerMovement;
    private CountDownHandler _countDownHandler;

    void Start()
    {
        _playerMovement = player.GetComponent<PlayerMovement>();
        _countDownHandler = countDown.GetComponent<CountDownHandler>();
        
        StartCoroutine(CountDown());
    }
    
    
    IEnumerator CountDown()
    {
        Debug.Log(_countDownHandler.GetTextInt());
        while (_countDownHandler.GetTextInt() > 0)
        {
            yield return new WaitForSecondsRealtime(2);
            _countDownHandler.Decrement();

        }

        _countDownHandler.Destroy();
        yield return new WaitForSecondsRealtime(1);
        _playerMovement.enabled = true;
    }

    public void End()
    {
        _playerMovement.enabled = false;
        DisplayUI();
    }

    private void DisplayUI()
    {
        
    }

}
