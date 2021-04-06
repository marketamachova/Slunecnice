using System.Collections.Generic;
using System.Linq;
using Network;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class BaseUIController : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private GameObject loadingSceneIndicator;

        [SerializeField] protected List<GameObject> panels;
        [SerializeField] private GameObject errorPanel;

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
            if (errorPanel)
            {
                errorPanel.SetActive(true);
            }
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

        private void Enable(string enalableName, bool enable)
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
    }
}