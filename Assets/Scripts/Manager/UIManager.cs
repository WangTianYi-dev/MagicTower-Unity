/*
 * file: UIManager.cs
 * author: DeamonHook
 * feature: UI管理器
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : BaseManager
{
    public static UIManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    // 游戏主窗口的快捷方式
    private GameWindow gameWindow;

    private PopMessage popMessage;

    private List<BaseWindow> Windows = new List<BaseWindow>();

    private Dictionary<string, BaseWindow> windowDict = new Dictionary<string, BaseWindow>();

    public GameObject floatMessageGO;

    public string currentWindow { get; private set; }

    private void RefreshProperty()
    {
        gameWindow.RefreshProperty(Player.instance.externalProperty);
    }

    private void RefreshEquipment()
    {
        gameWindow.RefreshEquipment(Player.instance.equipmentsWeared);
    }

    private void RefreshSkill()
    {
        gameWindow.RefreshSkill(Player.instance.curSkill);
    }

    public void RefreshUI()
    {
        RefreshProperty();
        RefreshEquipment();
        RefreshSkill();
    }

    public override void Init()
    {
        base.Init();
        instance = this;
        
        foreach (Transform window in transform.Find("Canvas"))
        {
            windowDict.Add(window.name, window.GetComponent<BaseWindow>());
        }
        foreach (var w in windowDict.Values)
        {
            w.InitWnd();
        }
        gameWindow = (GameWindow)windowDict["GameWindow"];

    }

    /// <summary>
    /// 根据名称切换窗口，并关闭其他窗口
    /// </summary>
    /// <param name="windowName">欲激活窗口的名称</param>
    public void SwitchWindow(string windowName)
    {
        foreach (var window in windowDict.Values)
        {
            window.SetActive(false);
        }
        windowDict[windowName].SetActive(true);
        currentWindow = windowName;
    }

    /// <summary>
    /// 根据名称激活窗口，不关闭其他窗口
    /// </summary>
    /// <param name="windowName">欲激活窗口的名称</param>
    public void OpenWindow(string windowName)
    {
        windowDict[windowName].SetActive(true);
        currentWindow = windowName;
    }

    

    public void PopMessage(string message)
    {
        gameWindow.ShowMessage(message);
    }

    /// <summary>
    /// 显示浮动信息（如伤害），默认产生位置为当前格子的中间
    /// </summary>
    public void FloatMessage(string message, Vector2Int logicPos)
    {
        //print($"FloatMessage: {message} at logicPos: {logicPos}");
        var obj = GameWindow.instance.gameArea.InstCanvasObject(floatMessageGO, logicPos, MapManager.instance.boxSize);
        obj.GetComponent<FloatMessage>().Refresh(message);
    }

    /// <summary>
    /// 刷新当前区域名称
    /// </summary>
    /// <param name="name"></param>
    public void RefreshAreaName(string name)
    {
        gameWindow.RefreshAreaName(name);
    }
}
