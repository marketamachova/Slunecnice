using System.Collections;
using UnityEngine;

namespace Animals
{
    public class AnimalTimer : MonoBehaviour
    {
        [SerializeField] private int waitingTime;
        private AnimalController _animalController;

        private void Start()
        {
            _animalController = GetComponent<AnimalController>();
            StartCoroutine(AnimateDelayed());
        }

        private IEnumerator AnimateDelayed()
        {
            yield return new WaitForSecondsRealtime(waitingTime);
            if (_animalController.GetMoving())
            {
                _animalController.TriggerMove();
            }
            _animalController.TriggerSound();
        }
    }
}
