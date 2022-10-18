/*
 * file: ManualWindow.cs
 * author: DeamonHook
 * feature: 图鉴窗口
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManualWindow : BaseWindow, IPointerClickHandler
{
    public GameObject manualEntry;

    private RectTransform panel, view;


    public static ManualWindow instance { get; private set; }


    public override void InitWnd()
    {
        base.InitWnd();
        instance = this;
    }


    private void Awake()
    {
        panel = transform.Find("Manual/Scroll View/Viewport/Content").GetComponent<RectTransform>();
        view = transform.Find("Manual/Scroll View").GetComponent<RectTransform>();
    }


    private void InitPanel()
    {
        for (int i = 0; i < panel.childCount; i++)
        {
            Destroy(panel.GetChild(i).gameObject);
        }
    }


    /// <summary>
    /// 刷新图鉴内容
    /// </summary>
    /// <param name="enemySet">当前区域的怪物集合</param>
    public void RefreshEntries(HashSet<Enemy> enemySet)
    {
        InitPanel();
        int len = enemySet.Count;
        panel.sizeDelta = new Vector2(panel.rect.width, Mathf.Max(view.rect.height, // 面板区域的高度
            (manualEntry.GetComponent<RectTransform>().rect.height + 4) * len));
        foreach (var enemy in enemySet)
        {
            GameObject entry = Util.Inst(manualEntry, panel, Vector2Int.zero);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<ManualEntry>().ShowInfo(enemy);
        }
    }


    public void OnPointerClick(PointerEventData ped)
    {
        UIManager.instance.SwitchWindow("GameWindow");
    }
}
