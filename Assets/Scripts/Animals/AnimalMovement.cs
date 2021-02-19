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
        private AnimalController _animalController;

        void Start()
        {
            animal.transform.position = pathCreator.path.GetPoint(0);
            _animalController = animal.GetComponent<AnimalController>();
        }

        void Update()
        {
            _distance += speed * Time.deltaTime;
            animal.transform.position = pathCreator.path.GetPointAtDistance(_distance, endOfPathInstruction);
            animal.transform.rotation = pathCreator.path.GetRotationAtDistance(_distance, endOfPathInstruction);
            
            if (_distance >= pathCreator.path.length)
            {
                if (_animalController != null)
                {
                    _animalController.TriggerStop();
                }
            }
        }
    }
}