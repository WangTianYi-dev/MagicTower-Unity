/*
 * file: ItemEntry.cs
 * author: DeamonHook
 * feature: 物品使用界面子项
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipEntry : MonoBehaviour, IPointerClickHandler
{
    // 内部所使用的名称
    public string itemName { get; private set; }

    public string intro;
    // 绑定到image组件上
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Refresh(string name)
    {
        itemName = name;
        intro = Equipment.EquipmentIntro[name.ToLower()];
        image.sprite = ResServer.instance.GetObject(name).GetComponent<SpriteRenderer>().sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EquipWindow.instance.SelectEquipment(this);
    }
}
