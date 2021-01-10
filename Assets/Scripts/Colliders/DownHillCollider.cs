using Cart;
using Player;
using UnityEngine;

namespace Colliders
{
    public class DownHillCollider : MonoBehaviour
    {
        public GameObject cart;
        public GameObject player;
        public bool end;
        private CartMovement _cartMovement;
        private PlayerMovement _playerMovement;

        void Start()
        {
            _cartMovement = cart.GetComponent<CartMovement>();
            _playerMovement = player.GetComponent<PlayerMovement>();
        }
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Terrain")
            {
                if (end)
                {
                    _playerMovement.UnShiftCamera();
                }
                // _cartMovement.FreezeRotation();
                _playerMovement.ShiftCamera();
            }
        }
    }
}
