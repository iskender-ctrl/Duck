using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableUI : Draggable
{
    public Image skillImage;

    // DraggableUI üzerindeki Image'ı güncelleyen fonksiyon
    public void SetUI(Sprite sprite)
    {
        if (skillImage != null)
        {
            skillImage.sprite = sprite;
        }
    }
}
