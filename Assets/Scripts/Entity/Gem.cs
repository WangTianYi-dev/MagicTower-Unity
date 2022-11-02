/*
 * file: Gem.cs
 * author: DeamonHook
 * feature: 物品
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Gem : Entity
{

	[Header("收集后的提示信息")]
	public string messageAfterCollect;

	/// <summary>
    /// 物品效果列表
    /// </summary>
	public List<string> additions;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		this.type = EntityType.Item;
		this.passable = true;
	}

	public override void AfterKilled()
	{
		base.AfterKilled();
        foreach (var addition in this.additions)
        {
            GameManager.instance.ApplyAddition(addition);
        }
        DestroySelf();
        UIManager.instance.PopMessage(messageAfterCollect);
    }

	public override void AfterMoveTo()
	{
		base.AfterMoveTo();
		AfterKilled();
	}
}

