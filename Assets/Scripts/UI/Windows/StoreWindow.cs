/*
 * file: MessageWindow.cs
 * author: DeamonHook
 * feature: 消息窗口
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEditor.Tilemaps;

//using DG.Tweening;
public class StoreWindow : BaseWindow, IPointerClickHandler
{
    public static StoreWindow instance {get; private set;}
    private GameObject messageGO;
    private GameObject imageGO;
    private RectTransform buttons;
    public GameObject button;

    public float padding = 30f;

    private void Awake()
    {
        instance = this;
        imageGO = transform.Find("Image").gameObject;
        messageGO = transform.Find("Text").gameObject;
        buttons = transform.Find("Buttons").GetComponent<RectTransform>();
    }

    List<Func<bool>> callBacks;

    private bool Invoke(int number)
    {
        if (callBacks[number]() == false)
        {
            UIManager.instance.PopMessage("交易失败");
            return false;
        }
        else
        {
            UIManager.instance.PopMessage("交易成功");
            return true;
        }
    }

    public void Refresh(string message, List<Func<bool>> callBacks, bool closeAfterDone=false)
    {
        this.callBacks = new List<Func<bool>>(callBacks);
        messageGO.GetComponent<Text>().text = message;
        StartCoroutine(FitSize());
        Util.ClearChildren(buttons);
        for (int i = 0; i < this.callBacks.Count; i++)
        {
            var t = i;
            var but = Util.Inst(button, buttons, Vector2Int.zero);
            but.GetComponent<ButtonA>().Bind(
                new Action
                (
                    () =>
                    {
                        print(t);
                        if (Invoke(t))
                        {
                            GameManager.instance.TriggerSuccess();
                            if (closeAfterDone)
                            {
                                UIManager.instance.SwitchWindow("GameWindow");
                            }
                            UIManager.instance.RefreshUI();
                        }
                    }
                ),
                (t+1).ToString()
            );
        }
    }

    IEnumerator FitSize()
    {
        yield return null;
        imageGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
        RectTransform.Axis.Vertical,
                messageGO.GetComponent<RectTransform>().sizeDelta.y + padding
            );
        imageGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
        RectTransform.Axis.Horizontal,
                messageGO.GetComponent<RectTransform>().sizeDelta.x + padding
            );
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.TriggerFailed();
        UIManager.instance.SwitchWindow("GameWindow");
    }
}