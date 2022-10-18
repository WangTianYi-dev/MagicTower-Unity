/*
 * file: ImageTextButton.cs
 * author: DeamonHook
 * feature: 带有图像和文字的按钮
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class ImageTextButton : MonoBehaviour, IPointerClickHandler
{
    private Text text;
    private Image image;

    private void Awake()
    {
        text = transform.GetComponentInChildren<Text>();
        image = transform.GetComponent<Image>();
    }

    private Action action;

    private Action callBack;

    public void Refresh(Sprite sprite, string msg, Action action)
    {
        image.sprite = sprite;
        callBack = null;
        text.text = msg;
        this.action = action;
    }

    public void Refresh(Sprite sprite, string msg, Action action, Action callBack)
    {
        image.sprite = sprite;
        text.text = msg;
        this.callBack = callBack;
        this.action = action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        action.Invoke();
        if (callBack != null)
        {
            callBack();
        }
    }
}
