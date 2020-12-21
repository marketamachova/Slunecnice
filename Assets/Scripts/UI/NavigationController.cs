using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NavigationController : MonoBehaviour
    {
        public GameObject homeScreen;
        public GameObject settingsScreen;
        public Sprite activeSprite;
        public Sprite inactiveSprite;
        private Image image;

        void Start()
        {
            image = GetComponentInChildren<Image>();
        }


        public void setHomeScreen()
        {
            homeScreen.SetActive(true);
            settingsScreen.SetActive(false);
            image.sprite = activeSprite;
        
        }

        public void setSettingsScreen()
        {
            homeScreen.SetActive(false);
            settingsScreen.SetActive(true);
            image.sprite = inactiveSprite;
        }
    }
}
