/*
 * file: MenuWindow.cs
 * author: DeamonHook
 * feature: 菜单窗口
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuWindow : BaseWindow, IPointerClickHandler
{
    public static MenuWindow instance { get; private set; }

    public override void InitWnd()
    {
        base.InitWnd();
        instance = this; 
    }

    public void OnPointerClick(PointerEventData ped)
    {
        UIManager.instance.SwitchWindow("GameWindow");
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnClearButtonClick()
    {
        GameManager.instance.DeleteAllArchives();
        UIManager.instance.SwitchWindow("GameWindow");
        UIManager.instance.PopMessage("存档已全部删除");
    }
}
