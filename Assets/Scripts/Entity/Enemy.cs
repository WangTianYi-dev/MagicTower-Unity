/*
 * file: Enemy.cs
 * author: DeamonHook
 * feature: 怪物
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Figure
{
    // 伤害
    public Int64 damage { get; private set; }

    // 打不过怪物时显示的信息
    public string blockedMessage = "你打不过此怪物！";

    protected override void Awake()
    {
        base.Awake();
        this.type = UnitType.Enemy;
        this.passable = false;
    }

    protected override void Start()
    {
        base.Start();
        if (this.effectAfterMoveTo == null)
        {
            this.effectAfterMoveTo = ResServer.instance.GetObject("DefeatedEffect");
        }
    }

    /// <summary>
    /// 刷新怪物伤害
    /// </summary>
    public void RefreshDamege()
    {
        damage = CombatCalc.CalcDamage(Player.instance, this);
    }



    public override void BeforeCollision()
    {
        base.BeforeCollision();
        RefreshDamege();
        if (0 < damage && damage < Player.instance.externalProperty.HP)
        {
            passable = true;
        }
        else
        { passable = false; }
    }


    public override void AfterMoveTo()
    {
        base.AfterMoveTo();
        // 这里直接减内部属性的生命值
        Player.instance.property.HP -= damage;
        Player.instance.property.Coin += this.property.Coin;
        DestroySelf();
        UIManager.instance.PopMessage($"{nameInGame}被打败了，金币+{property.Coin}");
    }

    public override void AfterBlocked()
    {
        base.AfterBlocked();
        UIManager.instance.PopMessage(blockedMessage);
    }
}
