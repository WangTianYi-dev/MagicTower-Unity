/*
 * file: ButtonA.cs
 * author: DeamonHook
 * feature: ”Îprefabs/ButtonA∞Û∂®
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonA : MonoBehaviour, IPointerClickHandler
{
    private Text text;

    private void Awake()
    {
        text = transform.Find("Number").GetComponent<Text>();
    }

    private Action callBack;

    public void Bind(Action call, string msg)
    {
        callBack = call;
        text.text = msg;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (callBack != null)
        {
            callBack();
        }
    }
    
}
