using Player;
using UnityEngine;

namespace Colliders
{
    public class EndCollider : MonoBehaviour
    {
        public GameObject player;
        private GameController _gameController;

        void Start()
        {
            _gameController = player.GetComponent<GameController>();
        }
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Terrain")
            {
                _gameController.End();
            }
        }
    }
}
