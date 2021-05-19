
using Animals;
using Player;
using UnityEngine;

namespace Colliders
{
    public class AnimalCollider : MonoBehaviour
    {
        [SerializeField] private GameObject animal;
        private AnimalController _controller;

        public void OnCollisionEnter(Collision collision)
        {
            _controller = animal.GetComponentInChildren<AnimalController>();
            if (collision.gameObject.CompareTag(GameConstants.NetworkCamera) && collision.gameObject.name != GameConstants.Terrain)
            {
                if (_controller.GetMoving())
                {
                    _controller.TriggerMove();
                }
                _controller.TriggerSound();
            }
        }
    }
}

