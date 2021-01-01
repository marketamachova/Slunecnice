using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownHandler : MonoBehaviour
{
    // public GameObject countDownText;
    public Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void Decrement()
    {
        var number = int.Parse(text.text);
        number--;
        text.text = number.ToString();
    }

    public int GetTextInt()
    {
        return int.Parse(text.text);
    }

    public void Destroy()
    {
        Destroy(text);
    }
    
}
