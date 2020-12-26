using System;
using System.Collections;
using System.Collections.Generic;
using Animals;
using UnityEngine;

namespace Colliders
{
    public class StopCollider : MonoBehaviour
    {
        public GameObject animal;
        private AnimalController _animalController;

        private void Start()
        {
            _animalController = animal.GetComponent<AnimalController>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Animal"))
            {
                _animalController.TriggerFadeOut();
            }
        }
    }
}