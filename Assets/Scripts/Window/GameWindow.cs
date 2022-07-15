/*
 * file: MessageWindow.cs
 * author: DeamonHook
 * date: 10/7/2022
 * feature: 游戏主窗口
 */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameWindow : BaseWindow
{
    /// <summary>
    /// 钥匙数量对应的文本对象
    /// </summary>
    private Text YellowKeyCount, BlueKeyCount, RedKeyCount;

    /// <summary>
    /// 玩家属性对应的文本对象
    /// </summary>
    private Text HP, ATK, DEF, Coin;

    /// <summary>
    /// 当前区域对应的文本对象
    /// </summary>
    private Text CurrentArea;

    /// <summary>
    /// 物品的图标
    /// </summary>
    private Image[] itemImages;

    private Text bottomMessage;

    public override void Init()
    {
        base.Init();
        YellowKeyCount = transform.Find("BackGround/Keys/YellowKeyCount").GetComponent<Text>();
        BlueKeyCount = transform.Find("BackGround/Keys/BlueKeyCount").GetComponent<Text>();
        RedKeyCount = transform.Find("BackGround/Keys/RedKeyCount").GetComponent<Text>();

        HP = transform.Find("BackGround/Stats/HP").GetComponent<Text>();
        ATK = transform.Find("BackGround/Stats/ATK").GetComponent<Text>();
        DEF = transform.Find("BackGround/Stats/DEF").GetComponent<Text>();
        Coin = transform.Find("BackGround/Stats/Coin").GetComponent<Text>();

        itemImages = transform.Find("BackGround/Items").GetComponentsInChildren<Image>();
        HideAllItems();

        bottomMessage = transform.Find("BackGround/Message").GetComponent<Text>();
    }

    /// <summary>
    /// 隐藏所有物品图标
    /// </summary>
    private void HideAllItems()
    {
        foreach (Image image in itemImages)
        {
            image.color = new Color(1, 1, 1, 0);
        }
    }

    /// <summary>
    /// 刷新主窗口所显示的玩家属性
    /// </summary>
    /// <param name="property">玩家的属性对象</param>
    public void RefreshStats(Property property)
    {
        YellowKeyCount.text = property.yellowKey.ToString();
        BlueKeyCount.text = property.blueKey.ToString();
        RedKeyCount.text = property.redKey.ToString();

        HP.text = property.HP.ToString();
        ATK.text = property.ATK.ToString();
        DEF.text = property.DEF.ToString();
    }

    public void RefreshItem(string name, int count) { }

    public void RefreshBottomMessage(string message)
    {
        bottomMessage.text = message;
    }
}