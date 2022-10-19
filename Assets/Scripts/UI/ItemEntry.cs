/*
 * file: ItemEntry.cs
 * author: DeamonHook
 * feature: ��Ʒʹ�ý�������
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemEntry : MonoBehaviour, IPointerClickHandler
{
    // �ڲ���ʹ�õ���Ʒ����
    public string itemName { get; private set; }
    // ��Ʒ����
    int count = 0;

    // �󶨵�image�����
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

    // ��ItemWindow����
    public void SelectMode()
    {
        image.color = new Color32(255, 255, 255, 255);
    }

    // ��ItemWindow����
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
