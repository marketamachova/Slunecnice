using System;
using System.Collections.Generic;
using Colliders;
using UnityEngine;

namespace Animals
{
    public class InstantiateAnimal : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private Vector3[] spawnPositions;
        [SerializeField] private GameObject parent;

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log("On collision Enter ");
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Terrain")
            {
                Debug.Log("On collision Enter PlAYER");

                for (int i = 0; i < prefabs.Length; i++)
                {
                    Instantiate(prefabs[i], parent.transform, false);
                }
            }
        }
    }
}