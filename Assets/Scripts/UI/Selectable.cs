using UnityEngine;
using UnityEngine.UI;


namespace UI
{

    /**
     * Changes (toggles) sprite of a UI element
     */
    public class Selectable : MonoBehaviour
    {
        [SerializeField] private string spriteName;
        private Image _image;
        private Sprite _sprite;
        private Sprite _spriteSelected;
        private void Start()
        {
            _image = GetComponent<Image>();
            _sprite = Resources.Load<Sprite>($"UI/{spriteName}");
            _spriteSelected = Resources.Load<Sprite>($"UI/{spriteName}Active");
        }

        public void SetSelected(bool selected)
        {
            _image.sprite = selected ? _spriteSelected : _sprite;
        }
        
    }
}