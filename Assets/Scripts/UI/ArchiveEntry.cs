/*
 * file: ArchiveEntry.cs
 * author: DeamonHook
 * feature: 存档界面项
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using Mono.Cecil;

public class ArchiveEntry : MonoBehaviour, IPointerClickHandler
{
    private Text index, currentArea, property;

    private int internalIndex; // 存档的内部序号


    private void Awake()
    {
        index = transform.Find("IndexText").GetComponent<Text>();
        currentArea = transform.Find("CurrentAreaText").GetComponent<Text>();
        property = transform.Find("PropertyText").GetComponent<Text>();
    }

    public enum Type
    {
        Load, Save
    }

    public Type type;

    public void Refresh(int idx, string area, string HP, string ATK, string DEF, string Coin)
    {
        internalIndex = idx;
        index.text = "存档" + idx.ToString();
        currentArea.text = $"当前区域：{area}";
        property.text = $"生命：{HP}，攻击：{ATK}，防御：{DEF}\n金币：{Coin}";
    }


    /// <summary>
    /// 创建新存档的按钮
    /// </summary>
    /// <param name="idx"></param>
    public void Refresh(int idx)
    {
        internalIndex = idx;
        index.text = "新存档";
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (type == Type.Load)
        {
            GameManager.instance.LoadGame(this.internalIndex);
            UIManager.instance.SwitchWindow("GameWindow");
            UIManager.instance.PopMessage($"存档{internalIndex}号读取成功！");
        }
        else if (type == Type.Save)
        {
            GameManager.instance.SaveGame(this.internalIndex);
            UIManager.instance.SwitchWindow("GameWindow");
            UIManager.instance.PopMessage($"存档{internalIndex}号保存成功！");
        }
        
    }
}
