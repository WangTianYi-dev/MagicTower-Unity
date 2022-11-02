/*
 * file: SkillEntry.cs
 * author: DeamonHook
 * feature: 物品使用界面子项
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillEntry : MonoBehaviour, IPointerClickHandler
{
    // 内部所使用的技能名称
    public string skillName { get; private set; }

    // 绑定到image组件上
    Image image;


    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Refresh(string name, Sprite source)
    {
        
        skillName = name;
        image.sprite = source;
    }

    // 由ItemWindow调用
    public void SelectMode()
    {
        image.color = new Color32(255, 255, 255, 255);
    }

    // 由ItemWindow调用
    public void DeselectMode()
    {
        image.color = new Color32(255, 255, 255, 127);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillWindow.instance.Select(this);
    }
}
