
using Animals;
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
            Debug.Log(_controller);
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Terrain")
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

