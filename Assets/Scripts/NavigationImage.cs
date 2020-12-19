using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationImage : MonoBehaviour
{
    public Image currentImage;
    public Sprite activeSprite;
    public Sprite inactiveSprite;


    public void OnPressedButton()
    {
        currentImage.sprite = activeSprite;
    }

    public void OnIdleButton()
    {
        currentImage.sprite = inactiveSprite;
    }
}
