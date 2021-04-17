using UnityEngine;

namespace UI
{
    public class UiElementConfig
    {
        public bool enable;
        public string elementName;

        public UiElementConfig(bool enable, string elementName)
        {
            this.enable = enable;
            this.elementName = elementName;
        }
    }
}