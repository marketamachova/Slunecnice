using System.Collections;
using UnityEngine;

namespace Utils
{
    public class FogController : MonoBehaviour
    {
        [SerializeField] private float fogDensity;


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.name != "Terrain")
            {
                SetFogAmount();
            }
        }

        public void SetFogAmount()
        {
            StartCoroutine(LessenFogDensity());
        }
        

        private IEnumerator LessenFogDensity()
        {
            var currentFogDensity = RenderSettings.fogDensity;
            while (currentFogDensity > fogDensity)
            {
                currentFogDensity -= 0.001f * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
                yield return null;
            }

            yield return null;
        }
    }
}