/*
 * file: GameArea.cs
 * author: DeamonHook
 * feature: 触控消息传递
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }


    public void OnPointerDown(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out position
        );

        float x = position.x / image.rectTransform.rect.width + 0.5f;
        float y = position.y / image.rectTransform.rect.height + 0.5f;
        GameManager.instance.TouchDown(x, y);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out position
        );

        float x = position.x / image.rectTransform.rect.width + 0.5f;
        float y = position.y / image.rectTransform.rect.height + 0.5f;
        GameManager.instance.TouchUp(x, y);
    }
}
