/*
 * file: EquipWindow.cs
 * author: DeamonHook
 * feature: 装备窗口
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipWindow: BaseWindow, IPointerClickHandler
{
    public static EquipWindow instance { get; private set; }

    public GameObject equipEntry;

    private Image weapon, weaponBox, armor, armorBox, treasure, treasureBox;

    private RectTransform equipListContent;

    private Text intro;

    private Equipment.EquipmentType curSelect;

    private void Awake()
    {
        instance = this;
        equipListContent = transform.Find("EquipList/Viewport/Content").GetComponent<RectTransform>();
        weapon = transform.Find("Window/CurrentEquipment/Weapon/Image").GetComponent<Image>();
        armor = transform.Find("Window/CurrentEquipment/Armor/Image").GetComponent<Image>();
        treasure = transform.Find("Window/CurrentEquipment/Treasure/Image").GetComponent<Image>();
        weaponBox = transform.Find("Window/CurrentEquipment/Weapon").GetComponent<Image>();
        weaponBox.color = Color.clear;
        armorBox = transform.Find("Window/CurrentEquipment/Armor").GetComponent<Image>();
        armorBox.color = Color.clear;
        treasureBox = transform.Find("Window/CurrentEquipment/Treasure").GetComponent<Image>();
        treasureBox.color = Color.clear;
        intro = transform.Find("Window/Introduction/Text").GetComponent<Text>();
        weaponBox.color = Color.white;
        curSelect = Equipment.EquipmentType.Weapon;
    }


    /// <summary>
    /// 选择一种类型
    /// </summary>
    /// <param name="t"></param>
    public void SelectType(string t)
    {
        weaponBox.color = Color.clear;
        armorBox.color = Color.clear;
        treasureBox.color = Color.clear;

        switch (t)
        {
            case "weapon":
                weaponBox.color = Color.white;
                curSelect = Equipment.EquipmentType.Weapon;
                break;
            case "armor":
                armorBox.color = Color.white;
                curSelect = Equipment.EquipmentType.Armor;
                break;
            case "treasure":
                treasureBox.color = Color.white;
                curSelect = Equipment.EquipmentType.Treasure;
                break;
        }
        RefreshWindow();
    }

    public void RefreshWindow()
    {
        weapon.color = Color.clear;
        armor.color = Color.clear;
        treasure.color = Color.clear;
        foreach (string e in Player.instance.equipmentsWeared)
        {
            var go = ResServer.instance.GetObject(e);
            Equipment eq = go.GetComponent<Equipment>();
            Sprite sp = go.GetComponent<SpriteRenderer>().sprite;
            switch (eq.equipmentType)
            {
                case Equipment.EquipmentType.Weapon:
                    weapon.sprite = sp;
                    weapon.color = Color.white;
                    break;
                case Equipment.EquipmentType.Armor:
                    armor.sprite = sp;
                    armor.color = Color.white;
                    break;
                case Equipment.EquipmentType.Treasure:
                    treasure.sprite = sp;
                    treasure.color = Color.white;
                    break;
            }
        }

        for (int i = 0; i < equipListContent.childCount; i++)
        {
            Destroy(equipListContent.GetChild(i).gameObject);
        }

        foreach (string str in Player.instance.equipments)
        {
            var e = ResServer.instance.GetObject(str);
            
            if (e.GetComponent<Equipment>().equipmentType == curSelect)
            {
                var go = Util.Inst(equipEntry, equipListContent, Vector2Int.zero);
                go.transform.localScale = Vector3.one;
                go.GetComponent<Image>().color = new Color32(255, 255, 255, 127);
                go.GetComponent<EquipEntry>().Refresh(str);
            }
        }
    }

    private void WearEquipment(EquipEntry ee)
    {
        intro.text = ee.intro;
        GameManager.instance.PlayerWearEquipment(ee.name);
    }


    private string selected;
    public void SelectEquipment(EquipEntry e)
    {
        if (selected != e.itemName)
        {
            selected = e.itemName;
            for (int i = 0; i < equipListContent.childCount; i++)
            {
                equipListContent.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 127);
            }
            intro.text = e.intro;
            e.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            GameManager.instance.PlayerWearEquipment(e.itemName);
            RefreshWindow();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.SwitchWindow("GameWindow");
    }
}

