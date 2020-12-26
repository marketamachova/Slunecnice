using System;
using System.Collections;
using System.Collections.Generic;
using Animals;
using UnityEngine;

namespace Colliders
{
    public class AnimalCollider : MonoBehaviour
    {
        public GameObject animal;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Terrain")
            {
                var controller = animal.GetComponent<AnimalController>();
                controller.TriggerMove();
            }
        }
    }
}

