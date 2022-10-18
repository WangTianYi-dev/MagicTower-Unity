/*
 * file: FlyWindow.cs
 * author: DeamonHook
 * feature: 
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Collections;


public class FlyWindow : BaseWindow, IPointerClickHandler
{
    public static FlyWindow instance {get; private set;}

    public List<Sprite> sprites;

    public GameObject imageTextButton;

    private Transform content;

    private void Awake()
    {
        instance = this;
        content = transform.Find("Manual/Scroll View/Viewport/Content");
    }

    public void Refresh(List<(string, Action)> msgs)
    {
        Util.ClearChildren(content);
        System.Random rand  = new System.Random();
        foreach (var item in msgs)
        {
            var go = Util.Inst(imageTextButton, content, Vector2Int.zero);
            go.GetComponent<ImageTextButton>().Refresh(sprites[rand.Next(0, sprites.Count)],
                item.Item1, item.Item2);
            go.transform.localScale = Vector3.one;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.SwitchWindow("GameWindow");
    }
}