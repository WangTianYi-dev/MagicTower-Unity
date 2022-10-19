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

public class ItemEntry : MonoBehaviour, IPointerClickHandler
{
    // 内部所使用的物品名称
    public string itemName { get; private set; }
    // 物品数量
    int count = 0;

    // 绑定到image组件上
    Image image;

    Text textCount;

    private void Awake()
    {
        image = GetComponent<Image>();
        textCount = transform.Find("Text").GetComponent<Text>();
    }

    public void Refresh(string name, int count, Sprite source)
    {
        
        itemName = name;
        this.count = count;
        image.sprite = source;
        if (count > 1)
        {
            textCount.text = count.ToString();
        }
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

    private void Update()
    {
        textCount.text = count > 1 ? count.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemWindow.instance.Select(this);
    }
}
