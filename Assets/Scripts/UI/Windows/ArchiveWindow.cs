/*
 * file: ArchiveWindow.cs
 * author: DeamonHook
 * feature: ��/��������
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ArchiveWindow : BaseWindow, IPointerClickHandler
{
    public GameObject archiveEntry;

    public static ArchiveWindow instance {get; private set;}

    private RectTransform content;

    private ArchiveEntry.Type entryType;

    private Text title, help;


    private void Awake()
    {
        instance = this;
        content = transform.Find("Scroll View/Content").GetComponent<RectTransform>();
        title = transform.Find("Title").GetComponent<Text>();
        help = transform.Find("Text").GetComponent<Text>();
    }

    private void Init()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    public void Refresh(List<MetaArchive> mas, ArchiveEntry.Type type)
    {
        Init();
        this.entryType = type;
        RefreshText();
        float height;
        if (this.entryType == ArchiveEntry.Type.Save)
        {
            height = (mas.Count + 1) * (archiveEntry.GetComponent<RectTransform>().rect.height + 5);
            content.sizeDelta = new Vector2(content.rect.width, height);
            GameObject e = Util.Inst(archiveEntry, content, new Vector2Int(0, 0));
            e.transform.localScale = Vector3.one;
            e.GetComponent<ArchiveEntry>().Refresh(mas.Count);
            e.GetComponent<ArchiveEntry>().type = ArchiveEntry.Type.Save;
            for (int i = mas.Count - 1; i >= 0; i--)
            {
                GameObject o = Util.Inst(archiveEntry, content, Vector2Int.zero);
                o.transform.localScale = Vector3.one;
                o.GetComponent<ArchiveEntry>().Refresh(mas[i].index, mas[i].areaName,
                    mas[i].HP, mas[i].ATK, mas[i].DEF, mas[i].Coin);
                o.GetComponent<ArchiveEntry>().type = ArchiveEntry.Type.Save;
            }
        }
        else if (this.entryType == ArchiveEntry.Type.Load)
        {
            height = mas.Count * (archiveEntry.GetComponent<RectTransform>().rect.height + 5);
            content.sizeDelta = new Vector2(content.rect.width, height);
            for (int i = mas.Count - 1; i >= 0; i--)
            {
                GameObject o = Util.Inst(archiveEntry, content, Vector2Int.zero);
                o.transform.localScale = Vector3.one;
                o.GetComponent<ArchiveEntry>().Refresh(mas[i].index, mas[i].areaName,
                    mas[i].HP, mas[i].ATK, mas[i].DEF, mas[i].Coin);
                o.GetComponent<ArchiveEntry>().type = ArchiveEntry.Type.Load;
            }
        }
    }




    private void RefreshText()
    {
        if (this.entryType == ArchiveEntry.Type.Save)
        {
            title.text = "存档";
            help.text = "点击以创建新存档或覆盖旧存档";
        }
        else if (this.entryType == ArchiveEntry.Type.Load)
        {
            title.text = "读档";
            help.text = "点击以读档";
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.SwitchWindow("GameWindow");
    }
}
