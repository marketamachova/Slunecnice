
using Animals;
using UnityEngine;

namespace Colliders
{
    public class AnimalCollider : MonoBehaviour
    {
        public GameObject animal;
        public GameObject animalPrefab;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Terrain")
            {
                // if (animal == null)
                // {
                //     Debug.Log("animal null");
                //     animal = Instantiate(animalPrefab, AnimalSpawnPositions.DeerSpawnPosition, Quaternion.identity);
                // }
                
                var controller = animal.GetComponent<AnimalController>();
                if (controller.moving)
                {
                    controller.TriggerMove();
                }
                controller.TriggerSound();
            }
        }
    }
}

