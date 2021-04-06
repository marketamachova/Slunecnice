using PathCreation;
using UnityEngine;

namespace Animals
{
    public class AnimalMovement : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed;
        private float _distance;
        private AnimalController _animalController;
        private bool _end = false;

        void Start()
        {
            transform.position = pathCreator.path.GetPoint(0);
            _animalController = GetComponent<AnimalController>();
        }

        void Update()
        {
            if (!_end)
            {
                _distance += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(_distance, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(_distance, endOfPathInstruction);

                if (_distance >= pathCreator.path.length)
                {
                    if (_animalController != null)
                    {
                        _animalController.TriggerStop();
                        _end = true;
                    }
                }
            }
        }
    }
}