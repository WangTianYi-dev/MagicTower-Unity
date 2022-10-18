/*
 * file: ManualWindow.cs
 * author: DeamonHook
 * feature: ͼ������
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemWindow : BaseWindow, IPointerClickHandler
{
    public GameObject itemEntryPrefab;

    public static ItemWindow instance { get; private set; }

    private ItemEntry curSelect = null;

    private RectTransform content;
    private Text introText;
    private GameObject obj;

    public override void InitWnd()
    {
        base.InitWnd();
        instance = this;
    }

    private void Awake()
    {
        //obj = transform.Find("Window").gameObject;
        //var t1 = transform.Find("Window/ItemList/Viewport/Content");
        //var t2 = transform.Find("Window/introduction/Text");
        //print(t1);
        //print(t2);
        content = transform.Find("Window/ItemList/Viewport/Content").GetComponent<RectTransform>();
        introText = transform.Find("Window/Introduction/Text").GetComponent<Text>();

    }

    public void ShowIntro(string intro)
    {
        introText.text = intro;
    }

    public void Select(ItemEntry item)
    {
        if (curSelect == item)
        {
            print($"Use {curSelect.itemName}");
            GameManager.instance.UseItem(curSelect.itemName);
            UIManager.instance.SwitchWindow("GameWindow");
        }
        else
        {
            if (curSelect != null)
            {
                curSelect.DeselectMode();
            }
            curSelect = item;
            item.SelectMode();
            print(item.intro);
            //ShowIntro(item.intro);
            ShowIntro("��ʵ�޻�����ǽ�䣬�����ƿ�һ��ǽ\n��һ�ξͻỵ���Ǹ�������̫���ǽ����̫���أ�");
        }
    }

    /// <summary>
    /// ������Ʒˢ�´���
    /// </summary>
    /// <param name="items">��ҵ���Ʒ��</param>
    public void Refresh(Dictionary<string, int> items)
    {
        foreach (var item in items)
        {
            print($"{item.Key}, {item.Value}");
        }
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        foreach (KeyValuePair<string, int> item in items)
        {
            if (item.Value > 0)
            {
                var entry = Instantiate(itemEntryPrefab, content.transform);
                var originalItem = ResServer.instance.GetObject(item.Key);
                entry.GetComponent<ItemEntry>().Refresh(
                    item.Key, item.Value,
                    originalItem.GetComponent<SpriteRenderer>().sprite,
                    originalItem.GetComponent<Item>().introText
                );
                entry.GetComponent<ItemEntry>().DeselectMode();
            }
        }
    }


    public void OnPointerClick(PointerEventData ped)
    {
        UIManager.instance.SwitchWindow("GameWindow");
    }
}
