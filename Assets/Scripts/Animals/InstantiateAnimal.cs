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

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Terrain")
            {
                for (int i = 0; i < prefabs.Length; i++)
                {
                    Instantiate(prefabs[i], spawnPositions[i], Quaternion.identity);
                }
            }
        }
    }
}