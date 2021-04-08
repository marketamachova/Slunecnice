using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Platform;
using TMPro;
using UnityEngine;

public class VRDebugger : MonoBehaviour
{
    public static VRDebugger Instance;
    [SerializeField] private TextMeshProUGUI text;
    private int _numLines = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Log(string message)
    {
        if (_numLines > 15)
        {
            _numLines = 0;
            text.text = message;
        }
        else
        {
            text.text += "\n" + message;
            _numLines++;
        }
    }
}
