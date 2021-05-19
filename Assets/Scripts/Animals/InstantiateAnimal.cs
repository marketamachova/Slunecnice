using System;
using System.Collections.Generic;
using Colliders;
using Player;
using UnityEngine;

namespace Animals
{
    public class InstantiateAnimal : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private GameObject parent;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(GameConstants.NetworkCamera) && collision.gameObject.name != GameConstants.Terrain)
            {
                for (int i = 0; i < prefabs.Length; i++)
                {
                    Instantiate(prefabs[i], parent.transform, false);
                }
            }
        }
    }
}