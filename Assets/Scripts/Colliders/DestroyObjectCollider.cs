using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colliders
{
    public class DestroyObjectCollider : MonoBehaviour
    {
        [SerializeField] private GameObject objectToDestroy;
        [SerializeField] private GameObject objectToSetActive;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("NetworkCamera") && !collision.gameObject.CompareTag("Terrain") && gameObject.name != "Terrain")
            {
                Destroy(objectToDestroy);
                if (objectToSetActive != null)
                {
                    objectToSetActive.SetActive(true);
                }
            }
        }
    }
}