/*
 * file: SimpleButton.cs
 * author: DeamonHook
 * feature: 简单的iOS风格纯文字按钮
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleButton : Button
{
    public Color originColor;
    public Color pressedColor;
    private Text[] texts;
    protected override void Awake()
    {
        base.Awake();
        originColor = new Color32(65, 105, 225, 255);
        pressedColor = new Color32(65, 105, 225, 127);
        texts = gameObject.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            text.color = originColor;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        foreach (var text in texts)
        {
            text.color = pressedColor;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        foreach (var text in texts)
        {
            text.color = originColor;
        }
    }
}

