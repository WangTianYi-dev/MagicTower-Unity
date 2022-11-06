/*
 * file: Combat.cs
 * author: DeamonHook
 * feature: 战斗相关类库
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInfo
{
    public long
        damage, //伤害
        round, //回合数
        threshold; //攻击临界
}


/// <summary>
/// 战斗相关计算
/// </summary>
public static class CombatCalc
{
    /// <summary>
    /// 基础的伤害计算(无任何技能)
    /// </summary>
    /// <param name="ATKerP">攻击者的属性</param>
    /// <param name="DEFerP">防御者的属性</param>
    /// <returns>若可以攻击则返回伤害，否则返回-1(无法攻击)</returns>
    private static Int64 BaseDamage(Property ATKerP, Property DEFerP)
    {
        // 无法攻击
        if (ATKerP.ATK <= DEFerP.DEF)
            return -1;
        else
        {
            Int64 ATKerADamage = ATKerP.ATK - DEFerP.DEF;
            Int64 DEFerADamage = Math.Max(DEFerP.ATK - ATKerP.DEF, 0);
            Int64 rounds = (Int64)Math.Ceiling((double)DEFerP.HP / ATKerADamage);
            return DEFerADamage * (rounds - 1);
        }
    }

    /// <summary>
    /// 图鉴中使用的伤害值（即不算BeforeBattle Buff的伤害）
    /// </summary>
    /// <param name="ATKer"></param>
    /// <param name="DEFer"></param>
    /// <returns></returns>
    public static long CalcLiteralDamage(Figure ATKer, Figure DEFer)
    {
        Property ATKerProperty = ATKer.externalProperty, DEFerProperty = DEFer.externalProperty;
        Debug.Log($"Player prop: {ATKerProperty}");
        return BaseDamage(ATKerProperty, DEFerProperty);
    }

    /// <summary>
    /// 计算伤害
    /// </summary>
    /// <param name="p1">攻击者属性</param>
    /// <param name="p2">防御者属性</param>
    /// <returns></returns>
    public static long CalcDamage(Property p1, Property p2)
    {
        return BaseDamage(p1, p2);
    }

    public static CombatInfo GetCombatInfo(Figure ATKer, Figure DEFer)
    {
        var info = new CombatInfo();
        Property ATKerProperty = ATKer.externalProperty, DEFerProperty = DEFer.externalProperty;

        // 每回合攻击者的伤害
        var uniDam = ATKerProperty.ATK - DEFerProperty.DEF;

        if (uniDam <= 0)
        {
            info.round = -1;
            info.threshold = DEFerProperty.DEF - ATKerProperty.ATK;
        }
        else
        {
            info.round = (long)Math.Ceiling((double)DEFerProperty.HP / uniDam) - 1;
            info.threshold = info.round > 1 ?
                (long)Math.Ceiling((double)DEFerProperty.HP / (info.round)) - uniDam
                : 0;
        }

        info.damage = BaseDamage(ATKerProperty, DEFerProperty);

        return info;
    }
}
