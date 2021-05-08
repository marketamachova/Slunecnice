using TMPro;
using UnityEngine;

namespace Utils
{
    public class VRDebugger : MonoBehaviour
    {
        public static VRDebugger Instance;
        [SerializeField] private TextMeshProUGUI text;
        private int _numLines = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void Log(string message)
        {
            if (_numLines > 15)
            {
                _numLines = 0;
                text.text = message;
            }
            else
            {
                text.text += "\n" + message;
                _numLines++;
            }
        }
    }
}
