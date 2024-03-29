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
    [HideInInspector]
    public long damage;

    // 打不过怪物时显示的信息
    public string blockedMessage = "你打不过此怪物！";

    protected override void Awake()
    {
        base.Awake();
        this.type = EntityType.Enemy;
        this.passable = false;
    }

    protected override void Start()
    {
        base.Start();
        if (this.effectAfterKilled == null)
        {
            this.effectAfterKilled = ResServer.instance.GetObject("DefeatedEffect");
        }
    }

    /// <summary>
    /// 刷新怪物伤害
    /// </summary>
    public void RefreshDamege()
    {
        damage = CombatCalc.CalcLiteralDamage(Player.instance, this);

    }

    public override void BeforeCollision()
    {
        base.BeforeCollision();
        //RefreshDamege();
        print($"{nameInGame}dam: {damage}");
        if (0 <= damage && damage < Player.instance.externalProperty.HP)
        {
            passable = true;
        }
        else
        { passable = false; }
    }

    public override void OnKilled()
    {
        base.OnKilled();
        // 这里直接减内部属性的生命值
        print("enemy attack");
        var dam = damage;
        Player.instance.property.HP -= dam;
        Player.instance.property.Coin += this.property.Coin;
        UIManager.instance.PopMessage($"{nameInGame}被打败了，金币+{property.Coin}");
        UIManager.instance.FloatMessage((-dam).ToString(), this.logicPos);
    }

    public override void AfterMoveTo()
    {
        base.AfterMoveTo();
    }

    public override void AfterBlocked()
    {
        base.AfterBlocked();
        UIManager.instance.PopMessage(blockedMessage);
    }
}
