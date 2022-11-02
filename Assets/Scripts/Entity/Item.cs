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
using System;

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
		internalName = ResServer.instance.GetEntityName(this).ToLower();
		this.type = EntityType.Item;
		this.introText = ItemIntro[internalName];
		this.passable = true;
		if (messageAfterCollect == "")
		{
			messageAfterCollect = $"获得{nameInGame}";
		}
		if (introText == null || introText == "")
			introText = ItemIntro[internalName.ToLower()];
	}

	public override void AfterKilled()
	{
		base.AfterKilled();
        GameManager.instance.GetItem(internalName);
        UIManager.instance.PopMessage(messageAfterCollect);
        GameManager.instance.RemoveEntity(this);
    }

	public override void AfterMoveTo()
	{
		base.AfterMoveTo();
		AfterKilled();
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
            "增加勇士（攻击+防御）* 10的生命\n这种质量的圣水，外面要卖1000金币，魔塔里却俯拾即是，难怪有人来这寻宝。喝下可以获得神的祝福，" +
            "当然，强大的人有资格获得更多的祝福。"
        }
    };

	public static Dictionary<string, Func<bool>> ItemEffects = new Dictionary<string, Func<bool>>
	{
		{
			"mattock",
			() =>
			{
				var mapmngr = MapManager.instance;
				var player = Player.instance;
				var uimngr = UIManager.instance;
				var facingPos = player.logicPos + player.playerDir;
				if (!mapmngr.PosInMap(facingPos))
				{
					uimngr.PopMessage("目标在地图外");
					return false;
				}
				if (mapmngr.groundEntityDict[facingPos].nameInGame == "墙")
				{
					mapmngr.ReplaceGroundEntity(facingPos, mapmngr.groundEntityDict[player.logicPos].gameObject);
					return true;
				}
				else
				{
					uimngr.PopMessage("镐只能对墙起作用");
					return false;
				}
			}
		},
		{
			"holywater",
            () =>
            {
                var mapmngr = MapManager.instance;
                var player = Player.instance;
                var uimngr = UIManager.instance;
                long amount = (player.property.DEF + player.property.ATK) * 10;
				uimngr.PopMessage($"使用圣水，增加{amount}生命");
				player.SetPropertyValue("hitpoint", amount + player.property.HP);
				uimngr.RefreshUI();
				return true;
            }
        }
	};
}