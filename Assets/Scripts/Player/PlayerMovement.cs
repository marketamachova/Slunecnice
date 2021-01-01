using PathCreation;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public GameObject player;
        public PathCreator pathCreator;
        public Vector3 offset = new Vector3(0, 10, 0);
        public float speed;
        private float _distance;

        void Start()
        {
            player.transform.position = pathCreator.path.GetPoint(0) + offset;
        }
    
        void Update()
        {
            _distance += speed * Time.deltaTime;
            player.transform.position = pathCreator.path.GetPointAtDistance(_distance) + offset;
        }
    }
}
