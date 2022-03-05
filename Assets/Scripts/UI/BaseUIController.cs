using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BaseUIController : MonoBehaviour
    {
        [Header("UI Elements")] 
        [SerializeField] private GameObject loadingSceneIndicator;
        [SerializeField] private TMP_Dropdown languagesDropdown;

        [SerializeField] protected List<GameObject> panels;
        [SerializeField] protected List<Selectable> activableSprites;
        [SerializeField] protected List<GameObject> enalableObjects;
        
        [SerializeField] protected MyNetworkManager networkManager;
        [SerializeField] protected BaseController controller;


        public void DisplayLoader(bool active)
        {
            loadingSceneIndicator.SetActive(active);
        }

        [ContextMenu("TriggerError")]
        public virtual void DisplayError()
        {
            EnableTrue(UIConstants.ErrorPanel);
            EnableTrue(UIConstants.ErrorBackdrop);
            EnableFalse(UIConstants.VideoControls);
            EnableFalse(UIConstants.SceneSelection);
            EnableFalse(UIConstants.Calibration);
            EnableFalse(UIConstants.SceneJoin);
            EnableFalse(UIConstants.Loader);
        }

        public void EnablePanelExclusive(string panelName)
        {
            panels.ForEach(panel => panel.SetActive(panel.name == panelName));
        }

        public void EnableTrue(string enalableName)
        {
            Enable(enalableName, true);
        }

        public void EnableFalse(string enalableName)
        {
            Enable(enalableName, false);
        }

        public virtual void ToggleEnable(string enalableName)
        {
            foreach (var enalable in enalableObjects.Where(enalable => enalable.name == enalableName))
            {
                var enable = enalable.activeSelf;
                enalable.SetActive(!enable);
            }
        }

        public void ActivateExclusive(string activableName)
        {
            activableSprites.ForEach(activable => activable.SetSelected(activable.name == activableName));
        }

        public void Activate(string activableName)
        {
            ActivateSprite(activableName, true);
        }

        public void Deactivate(string activableName)
        {
            ActivateSprite(activableName, false);
        }

        public void OnQuit()
        {
            Application.Quit();
        }

        protected void Enable(string enalableName, bool enable)
        {
            foreach (var enalable in enalableObjects.Where(enalable => enalable.name == enalableName))
            {
                enalable.SetActive(enable);
            }
        }

        private void ActivateSprite(string activableName, bool activate)
        {
            foreach (var selectable in activableSprites.Where(selectable => selectable.name == activableName))
            {
                selectable.SetSelected(activate);
            }
        }
        
        protected IEnumerator ActivateButtons(Button[] buttons, int time, bool interactable)
        {
            yield return new WaitForSecondsRealtime(time);

            foreach (var button in buttons)
            {
                button.interactable = interactable;
            }
        }

        public void OnLanguageSelected()
        {
            controller.OnLanguageSelected(languagesDropdown.value);
        }

        public void SetLanguageDropdownValue(int index)
        {
            languagesDropdown.value = index;
        }
        
    }
}