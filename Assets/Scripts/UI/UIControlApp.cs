using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIControlApp : MonoBehaviour
    {
        [SerializeField] Button joinButton;

        public void Join()
        {
            //check if join was sucessful
            joinButton.interactable = false;

            Client.localClient.JoinGame();
        }
    }
}
