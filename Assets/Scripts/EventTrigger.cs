using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 FROG_TRIGGER_POSITION = new Vector3(107, 113, 180);
        
        if (player.transform.position.x >= FROG_TRIGGER_POSITION.x 
            && player.transform.position.z >= FROG_TRIGGER_POSITION.z)
        {
            
        }
    }
}
