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

public class EquipEntry : MonoBehaviour, IPointerClickHandler
{
    // �ڲ���ʹ�õ�����
    public string itemName { get; private set; }

    public string intro;
    // �󶨵�image�����
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
