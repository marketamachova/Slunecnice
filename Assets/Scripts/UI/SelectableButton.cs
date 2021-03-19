using System;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

namespace UI
{
    enum ButtonType
    {
        PlayerCameraButton, 
        TopCameraButton, 
        DashboardButton
    }

    public class SelectableButton : MonoBehaviour
    {
        [SerializeField] private ButtonType buttonType;
        private Button _button;
        private Sprite _buttonSprite;
        private Sprite _buttonSpriteSelected;
        private void Start()
        {
            _button = GetComponent<Button>();
            _buttonSprite = Resources.Load<Sprite>($"UI/{buttonType}");
            _buttonSpriteSelected = Resources.Load<Sprite>($"UI/{buttonType}Active");
        }

        public void SetSelected(bool selected)
        {
            _button.image.sprite = selected ? _buttonSpriteSelected : _buttonSprite;
        }
        
    }
}