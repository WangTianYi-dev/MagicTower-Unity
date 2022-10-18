/*
 * file: ArchiveEntry.cs
 * author: DeamonHook
 * feature: �浵������
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

    private int internalIndex; // �浵���ڲ����


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
        index.text = "�浵" + idx.ToString();
        currentArea.text = $"��ǰ����{area}";
        property.text = $"������{HP}��������{ATK}��������{DEF}\n��ң�{Coin}";
    }


    /// <summary>
    /// �����´浵�İ�ť
    /// </summary>
    /// <param name="idx"></param>
    public void Refresh(int idx)
    {
        internalIndex = idx;
        index.text = "�´浵";
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (type == Type.Load)
        {
            GameManager.instance.LoadGame(this.internalIndex);
            UIManager.instance.SwitchWindow("GameWindow");
            UIManager.instance.PopMessage($"�浵{internalIndex}�Ŷ�ȡ�ɹ���");
        }
        else if (type == Type.Save)
        {
            GameManager.instance.SaveGame(this.internalIndex);
            UIManager.instance.SwitchWindow("GameWindow");
            UIManager.instance.PopMessage($"�浵{internalIndex}�ű���ɹ���");
        }
        
    }
}
