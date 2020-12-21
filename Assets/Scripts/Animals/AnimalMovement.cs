using PathCreation;
using UnityEngine;

namespace Animals
{
    public class AnimalMovement : MonoBehaviour
    {
        public GameObject animal;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed;
        private float _distance;

        void Start()
        {
            Debug.Log("AnimalMovement enabled");
            animal.transform.position = pathCreator.path.GetPoint(0);
        }

        void Update()
        {
            _distance += speed * Time.deltaTime;
            animal.transform.position = pathCreator.path.GetPointAtDistance(_distance, endOfPathInstruction);
        }
    }
}
