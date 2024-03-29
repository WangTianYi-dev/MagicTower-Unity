﻿/*
 * file: Item.cs
 * author: DeamonHook
 * feature: 物品（可使用的）
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Buff;

public class Equipment : Entity
{
	[Header("收集后的提示信息")]
	public string messageAfterCollect;

	[Header("内部名称（与prefab名称一致）")]
	public string internalName;

    [Header("装备类型")]
    public EquipmentType equipmentType;

	public enum EquipmentType
	{
		Weapon, // 武器
		Armor, // 防具
        Treasure // 宝物    
	}

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
        if (internalName == "")
        {
            internalName = ResServer.instance.GetEntityName(this);
        }
		internalName = internalName.ToLower();
		this.type = EntityType.Item;
		this.passable = true;
	}

    public override void OnKilled()
    {
        base.OnKilled();
        Player.instance.equipments.Add(internalName);
        UIManager.instance.PopMessage(messageAfterCollect);
        GameManager.instance.RemoveEntity(this);
    }

    public override void AfterMoveTo()
	{
		base.AfterMoveTo();
        KillSelf();
	}

    
    // 效果设定
    public static Dictionary<string, List<Buff>> EquipmentBuffDict = new Dictionary<string, List<Buff>>
    {
        {
            // 铁剑
            "ironsword",
            new List<Buff> {
                new Buff(
                    AdditionType.Sustain,
                    LaunchTime.AllTime,
                    new UnaryAddition(
                        (ref Property self) =>
                        {
                            self.ATK += (long)(self.ATK * 0.1);
                        }
                    ),
                    null
                )
            }
        },
        {
            // 铁盾
            "ironshield",
            new List<Buff> {
                new Buff(
                    AdditionType.Sustain,
                    LaunchTime.AllTime,
                    new UnaryAddition(
                        (ref Property self) =>
                        {
                            self.DEF += (long)(self.DEF * 0.1);
                        }
                    ),
                    null
                )
            }
        },
        {
            // 智慧权杖
            "intellion",
            new List<Buff> {
                new Buff(
                    AdditionType.Sustain,
                    LaunchTime.AllTime,
                    new UnaryAddition(
                        (ref Property self) =>
                        {
                            self.ATK = (long)(self.ATK * 0.1);
                        }
                    ),
                    null
                ),
                new Buff(
                    AdditionType.Sustain,
                    LaunchTime.BeforeBattle,
                    null,
                    new BinaryAddition(
                        (ref Property self, ref Property enemy) =>
                        {
                            self.ATK += enemy.DEF;
                        }
                    )
                ),
            }
        },
        {
            // 刺剑
            "rapier",
            new List<Buff> {
                new Buff(
                    AdditionType.Sustain,
                    LaunchTime.BeforeBattle,
                    null,
                    new BinaryAddition(
                        (ref Property self, ref Property enemy) =>
                        {
                            if (self.ATK > enemy.DEF)
                            {
                                enemy.HP = Math.Max(enemy.HP - (self.ATK - enemy.DEF), 1);
                            }
                        }
                        )
                    )
            }
        }
    };

    // 介绍信息，key为小写
    public static Dictionary<string, string> EquipmentIntro = new Dictionary<string, string>
    {
        {
            "ironsword",
            "【普通】+10% 攻击\n人族军队的制式武器，为什么会被严格看守起来呢？"
        },
        {
            "intellion",
            "【神器】-90% 攻击，攻击无视对方防御\n帝国的镇国之宝，人类智慧的象征。可是，为什么会出现在这里？"
        },
        {
            "ironshield",
            "【普通】+10% 防御\n比铁剑贵重，其实战场上并不常用，因为它又贵又重"
        },
        {
            "rapier",
            "【史诗】战斗前进行一次额外攻击\n在芝诺还不是魔法警卫的时候，这把刺剑伴随他走过十年的佣兵生涯，" +
            "握住它似乎可以听到冤魂凄厉的叫声，现在它被赠予勇士，作为二人友谊的见证"
        }
    };
}