using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /**
     * Manages displaying of Loader UI element
     */
    public class LoaderUI : MonoBehaviour
    {
        [SerializeField] private GameObject loader;
        private Slider _loaderSliderComponent;

        private void Start()
        {
            _loaderSliderComponent = loader.GetComponent<Slider>();
        }

        public void UpdateLoader(float progress)
        {
            _loaderSliderComponent.value = Mathf.Clamp01(progress / 0.9f);
        }

        public void DisplayLoader(bool display)
        {
            loader.SetActive(display);
        }
        
        
    }
}