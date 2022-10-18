/*
 * file: Item.cs
 * author: DeamonHook
 * feature: 物品（可使用的）
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class Item : Entity
{
	[Header("收集后的提示信息")]
	public string messageAfterCollect;

	[Header("介绍文本")]
	public string introText;

	private string internalName;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		internalName = ResServer.instance.GetEntityName(this);
		this.type = UnitType.Item;
		this.passable = true;
		if (introText == null || introText == "")
			introText = ItemIntro[internalName.ToLower()];
	}

	public override void AfterMoveTo()
	{
		base.AfterMoveTo();
		GameManager.instance.GetItem(internalName);
		UIManager.instance.PopMessage(messageAfterCollect);
		GameManager.instance.RemoveEntity(this);
	}

    // key为小写
    public static Dictionary<string, string> ItemIntro = new Dictionary<string, string>
    {
        {
            "mattock",
            "朴实无华的破墙镐，可以破开一堵墙\n用一次就会坏，是镐子质量太差还是墙质量太好呢？"
        },
        {
            "holywater",
            "增加勇士（攻击+防御）* 10的生命\n这种质量的圣水，外面要卖1000金币，魔塔里却俯拾即是，难怪有人来这寻宝"
        }
    };
}