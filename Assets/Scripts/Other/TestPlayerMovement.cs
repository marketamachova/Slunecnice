using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestPlayerMovement : NetworkBehaviour
{
  void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal") * 0.5f;
            float moveVertical = Input.GetAxis("Vertical") * 0.5f;

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
            transform.position = transform.position + movement;
        }
    }

    private void Update()
    {
        HandleMovement();
    }
}
