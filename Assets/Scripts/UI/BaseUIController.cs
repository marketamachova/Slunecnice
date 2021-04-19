using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class BaseUIController : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private GameObject loadingSceneIndicator;

        [SerializeField] protected List<GameObject> panels;

        [FormerlySerializedAs("activables")] [SerializeField]
        protected List<Selectable> activableSprites;

        [FormerlySerializedAs("enalables")] [SerializeField]
        protected List<GameObject> enalableObjects;

        [SerializeField] protected MyNetworkManager networkManager;


        public void DisplayLoader(bool active)
        {
            loadingSceneIndicator.SetActive(active);
        }

        public virtual void DisplayError()
        {
            EnableTrue("ErrorPanel");
            EnableTrue("ErrorBackdrop");
            EnableFalse("VideoControls");
            EnableFalse("SceneSelection");
            EnableFalse("Calibration");
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

        public void ToggleEnable(string enalableName)
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

        //todo mozna presunout do Controller
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
        
    }
}