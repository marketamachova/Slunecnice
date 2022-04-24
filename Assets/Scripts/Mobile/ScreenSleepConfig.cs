using UnityEngine;

public class ScreenSleepConfig : MonoBehaviour
{
    // Sets screen to never dim
    void Start()
    { 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}

