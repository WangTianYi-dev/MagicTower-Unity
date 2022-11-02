/*
 * file: SkillEntry.cs
 * author: DeamonHook
 * feature: ��Ʒʹ�ý�������
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillEntry : MonoBehaviour, IPointerClickHandler
{
    // �ڲ���ʹ�õļ�������
    public string skillName { get; private set; }

    // �󶨵�image�����
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

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillWindow.instance.Select(this);
    }
}
