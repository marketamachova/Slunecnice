using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colliders
{
    public class DestroyObjectCollider : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objectsToDestroy;
        [SerializeField] private List<GameObject>  objectsToSetActive;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("NetworkCamera") && !collision.gameObject.CompareTag("Terrain") && gameObject.name != "Terrain")
            {
                objectsToDestroy.ForEach(Destroy);
                objectsToSetActive.ForEach(o => o.SetActive(true));
            }
        }
    }
}