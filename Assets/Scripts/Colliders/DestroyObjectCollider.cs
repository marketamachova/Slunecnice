using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Colliders
{
    public class DestroyObjectCollider : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objectsToDestroy;
        [SerializeField] private List<GameObject>  objectsToSetActive;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(GameConstants.NetworkCamera) && !collision.gameObject.CompareTag(GameConstants.Terrain) && gameObject.name != GameConstants.Terrain)
            {
                objectsToSetActive.ForEach(o => o.SetActive(true));
                objectsToDestroy.ForEach(Destroy);
            }
        }
    }
}