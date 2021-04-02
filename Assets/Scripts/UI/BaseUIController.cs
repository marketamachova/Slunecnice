using System.Collections.Generic;
using System.Linq;
using Network;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class BaseUIController : MonoBehaviour
    {
        [SerializeField] private GameObject loadingSceneIndicator;
        [SerializeField] protected List<GameObject> panels;
        [FormerlySerializedAs("activables")] [SerializeField] protected List<Selectable> activableSprites;
        [FormerlySerializedAs("enalables")] [SerializeField] protected List<GameObject> enalableObjects;
        [SerializeField] protected MyNetworkManager networkManager;
        
        [SerializeField] private GameObject errorPanel;
        
        public void DisplayLoader()
        {
            loadingSceneIndicator.SetActive(true);
        }

        public virtual void DisplayError()
        {
            errorPanel.SetActive(true);
        }
        
        public void EnablePanelExclusive(string panelName)
        {
            panels.ForEach(panel => panel.SetActive(panel.name == panelName));
        }

        public void EnableTrue(string enalableName)
        {
            foreach (var enalable in enalableObjects.Where(enalable => enalable.name == enalableName))
            {
                enalable.SetActive(true);
            }
        }
        
        public void EnableFalse(string enalableName)
        {
            foreach (var enalable in enalableObjects.Where(enalable => enalable.name == enalableName))
            {
                enalable.SetActive(false);
            }
        }
        
        public void ActivateExclusive(string activableName)
        {
            activableSprites.ForEach(activable => activable.SetSelected(activable.name == activableName));
        }

        public void Activate(string activableName)
        {
            foreach (var selectable in activableSprites.Where(selectable => selectable.name == activableName))
            {
                selectable.SetSelected(true);
            }
        }
        
        public void Deactivate(string activableName)
        {
            foreach (var selectable in activableSprites.Where(selectable => selectable.name == activableName))
            {
                selectable.SetSelected(false);
            }
        }
    }
}
