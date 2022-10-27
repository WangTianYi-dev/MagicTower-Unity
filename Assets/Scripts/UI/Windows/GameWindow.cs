/*
 * file: MessageWindow.cs
 * author: DeamonHook
 * date: 10/7/2022
 * feature: 游戏主窗口
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Globalization;

//using DG.Tweening;
public class GameWindow : BaseWindow
{
    public static GameWindow instance { get; private set; }



    /// <summary>
    /// 钥匙数量对应的文本对象
    /// </summary>
    private Text YellowKeyCount, BlueKeyCount, RedKeyCount;

    /// <summary>
    /// 玩家属性对应的文本对象
    /// </summary>
    private Text HPtxt, ATKtxt, DEFtxt, Cointxt;

    /// <summary>
    /// 当前区域对应的文本对象
    /// </summary>
    private Text CurrentArea;

    private Image weapon, armor, skill;

    private Text bottomMessage;

    private PopMessage popMessage;

    public GameArea gameArea { get; private set; }

    // 区域名称
    private Text areaName;

    public override void InitWnd()
    {
        base.InitWnd();

        instance = this;

        // 异形屏适配
        var safeArea = Screen.safeArea;
        var leftPinRectTran = transform.Find("LeftPin").GetComponent<RectTransform>();
        var rightPinRectTran = transform.Find("RightPin").GetComponent<RectTransform>();
        leftPinRectTran.anchoredPosition = new Vector2(safeArea.xMin, 0);
        rightPinRectTran.anchoredPosition = new Vector2(safeArea.xMax - Screen.width, 0);

        YellowKeyCount = transform.Find("LeftPin/Keys/YellowKeyCount").GetComponent<Text>();
        BlueKeyCount = transform.Find("LeftPin/Keys/BlueKeyCount").GetComponent<Text>();
        RedKeyCount = transform.Find("LeftPin/Keys/RedKeyCount").GetComponent<Text>();

        HPtxt = transform.Find("RightPin/Stats/HPText").GetComponent<Text>();
        ATKtxt = transform.Find("RightPin/Stats/ATKText").GetComponent<Text>();
        DEFtxt = transform.Find("RightPin/Stats/DEFText").GetComponent<Text>();
        Cointxt = transform.Find("RightPin/Stats/CoinText").GetComponent<Text>();

        popMessage = transform.Find("CenterPin/PopMessage").GetComponent<PopMessage>();
        areaName = transform.Find("RightPin/CurrentArea").GetComponent<Text>();

        weapon = transform.Find("LeftPin/Equip/SwordImage").GetComponent<Image>();
        armor = transform.Find("LeftPin/Equip/ShieldImage").GetComponent<Image>();
        skill = transform.Find("LeftPin/Equip/SkillImage").GetComponent<Image>();

        gameArea = transform.Find("CenterPin/GameArea").GetComponent<GameArea>();
    }


    /// <summary>
    /// 刷新主窗口所显示的玩家属性
    /// </summary>
    /// <param name="property">玩家的属性对象</param>
    public void RefreshProperty(Property property)
    {
        YellowKeyCount.text = property.yellowKey.ToString();
        BlueKeyCount.text = property.blueKey.ToString();
        RedKeyCount.text = property.redKey.ToString();

        HPtxt.text = property.HP.ToString();
        ATKtxt.text = property.ATK.ToString();
        DEFtxt.text = property.DEF.ToString();
        Cointxt.text = property.Coin.ToString();
    }


    public void RefreshEquipment(IEnumerable<string> strs)
    {

        foreach (var name in strs)
        {
            var go = ResServer.instance.GetObject(name);
            var e = go.GetComponent<Equipment>();
            switch (e.equipmentType)
            {
                case Equipment.EquipmentType.Weapon:
                    weapon.color = Color.white;
                    weapon.sprite = go.GetComponent<SpriteRenderer>().sprite;
                    break;
                case Equipment.EquipmentType.Armor:
                    armor.color = Color.white;
                    armor.sprite = go.GetComponent<SpriteRenderer>().sprite;
                    break;
            }
            
        }
    }

    public void RefreshSkill(string skillName)
    {
        if (skillName == "")
        {
            skill.color = Color.clear;
        }
        else
        {
            var go = ResServer.instance.GetObject(skillName);
            skill.sprite = go.GetComponent<SpriteRenderer>().sprite;
            skill.color = Color.white;
        }
    }

    /// <summary>
    /// 刷新区域名称
    /// </summary>
    /// <param name="name"></param>
    public void RefreshAreaName(string name)
    {
        areaName.text = name;
    }

    /// <summary>
    /// 在底栏上显示信息
    /// </summary>
    /// <param name="message">信息</param>
    public void ShowMessage(string message)
    {
        popMessage.ShowMsg(message);
    }

    public void RefreshItem() { }

    /// <summary>
    /// 设置按钮的点击回调
    /// </summary>
    public void OnSettingButtonClick()
    {
        UIManager.instance.OpenWindow("MenuWindow");
    }

    public void OnManualButtonClick()
    {
        UIManager.instance.OpenWindow("ManualWindow");
        var s = GameManager.instance.GetEnemys();
        ManualWindow.instance.RefreshEntries(s);
    }

    public void OnItemButtonClicked()
    {
        UIManager.instance.OpenWindow("ItemWindow");
        ItemWindow.instance.Refresh(Player.instance.items);
        ItemWindow.instance.ShowIntro("勇者打开了他的四次元口袋！");
    }

    public void OnSkillButtonClicked()
    {
        UIManager.instance.OpenWindow("SkillWindow");
        SkillWindow.instance.Refresh(Player.instance.skills);
        SkillWindow.instance.ShowIntro("勇士屏息静气，准备发动技能");
    }

    public void OnSaveButtonClicked()
    {
        UIManager.instance.OpenWindow("ArchiveWindow");
        ArchiveWindow.instance.Refresh(GameManager.instance.GetMetasOrdered(), ArchiveEntry.Type.Save);
    }


    public void OnLoadButtonClicked()
    {
        
        UIManager.instance.OpenWindow("ArchiveWindow");
        ArchiveWindow.instance.Refresh(GameManager.instance.GetMetasOrdered(), ArchiveEntry.Type.Load);
    }

    public void OnEquipButtonClicked()
    {
        UIManager.instance.OpenWindow("EquipWindow");
        EquipWindow.instance.RefreshWindow();
    }

    public void OnFlyButtonCLicked()
    {
        UIManager.instance.OpenWindow("FlyWindow");
        var flyList = GameManager.instance.GetFlyList();
        FlyWindow.instance.Refresh(flyList);
    }

    public void OnOtherButtonClicked()
    {
        UIManager.instance.PopMessage("暂未实现此功能");
    }
    
    
}