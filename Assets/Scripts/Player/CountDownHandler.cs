using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownHandler : MonoBehaviour
{
    public GameObject countDownText;
    public Text Text { get; set; }

    private void Start()
    {
        Text = countDownText.GetComponent<Text>();
    }

    public void Decrement()
    {
        var number = int.Parse(Text.text);
        number--;
        Text.text = number.ToString();
    }

    public int GetTextInt()
    {
        return int.Parse(Text.text);
    }

    public void Destroy()
    {
        Destroy(Text);
    }
    
}
