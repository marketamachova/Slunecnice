using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public PathCreator pathCreator;
    public float speed;
    float distance;

    void Start()
    {
        player.transform.position = pathCreator.path.GetPoint(10);
    }


    // Update is called once per frame
    void Update()
    {
        distance += speed * Time.deltaTime;
        player.transform.position = pathCreator.path.GetPointAtDistance(distance);
    }
}
