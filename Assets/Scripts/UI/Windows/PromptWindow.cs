/*
 * file: PromptWindow.cs
 * author: DeamonHook
 * feature: ÌáÊ¾´°¿Ú
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PromptWindow : BaseWindow, IPointerClickHandler
{
    public static PromptWindow instance { get; private set; }

    public GameObject textGO, imageGO;

    private List<string> content = new List<string>();
    private int index = 0;

    private readonly float padding = 30f;

    public override void InitWnd()
    {
        base.InitWnd();
        instance = this;
    }

    public void RefreshPrompt(List<string> dialog)
    {
        content = new List<string>(dialog);
        index = 0;
        ShowNextSentence();
    }

    public bool ShowNextSentence()
    {
        if (index < content.Count)
        {
            textGO.GetComponent<Text>().text = content[index];
            index++;
            StartCoroutine(FitSize());
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator FitSize()
    {
        yield return null;
        imageGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                textGO.GetComponent<RectTransform>().sizeDelta.y + padding
            );
        imageGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                textGO.GetComponent<RectTransform>().sizeDelta.x + padding
            );
    }

    public void OnPointerClick(PointerEventData ped)
    {
        if (!ShowNextSentence())
        {
            GameManager.instance.CurrentEventDone();
            //GameManager.instance.TriggerSuccess();
            UIManager.instance.SwitchWindow("GameWindow");
        }
    }
}
