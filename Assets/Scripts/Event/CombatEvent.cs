/*
 * file: CombatEvent.cs
 * author: DeamonHook
 * date: 13/6/2022
 * feature: 战斗事件
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatEvent : MonoBehaviour
{

    public static CombatEvent NewCombat(Unit ATKer, Unit DEFer)
    {
        CombatEvent c = new CombatEvent();
        c.ATKer = ATKer;
        c.DEFer = DEFer;
        return c;
    }

    /// <summary>
    /// 先手攻击者和后手防御者
    /// </summary>
    private Unit ATKer, DEFer;

    /// <summary>
    /// 基础的伤害计算(无任何技能)
    /// </summary>
    /// <param name="ATKerP">攻击者的属性</param>
    /// <param name="DEFerP">防御者的属性</param>
    /// <returns>若可以攻击则返回伤害，否则返回-1(无法攻击)</returns>
    public Int64 BaseDamage(Property ATKerP, Property DEFerP)
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
}
