/*
 * file: Addition.cs
 * author: DeamonHook
 * feature: 加成效果
 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;

// 二元的加成效果
public delegate void BinaryAddition(ref Property self, ref Property enemy);

// 一元的加成效果
public delegate void UnaryAddition(ref Property self);

public class Buff
{
    /// <summary>
    /// 瞬时效果，战斗计算时调用
    /// </summary>
    public static Dictionary<string, BinaryAddition> instantDict = new Dictionary<string, BinaryAddition>
    {
        {
            "intellion", new BinaryAddition(IntellionA)
        }
    };

    /// <summary>
    /// 持续效果，
    /// </summary>
    public static Dictionary<string, UnaryAddition> sustainDict = new Dictionary<string, UnaryAddition>
    {
        {
            "ironsword", new UnaryAddition(IronSword)
        },
        {
            "ironshield", new UnaryAddition(IronShield)
        },
        {
            "intellion", new UnaryAddition(IntellionB)
        }
    };

    public static void IronSword(ref Property self)
    {
        self.ATK += (long)(self.ATK * 0.1);
    }

    public static void IronShield(ref Property self)
    {
        self.DEF += (long)(self.DEF * 0.1);
    }

    public static void IntellionA(ref Property self, ref Property enemy)
    {
        self.ATK += enemy.DEF;
    }

    public static void IntellionB(ref Property self)
    {
        self.ATK = (long)(self.ATK * 0.1);
    }

    /*
     * 将加成效果分为5类
     * 1. 永久加成，如宝石、血瓶
     * 2. 持续加成，战斗前生效，如剑/盾
     * 3. 持续加成，战斗后生效，如幸运金币
     * 4. 瞬时加成，战斗前生效，如凡骨、皇者
     * 5. 瞬时加成，战斗后生效，如流石
     */

    public enum AdditionType
    {
        Persistant, // 永久加成，不能取消
        Sustain, // 持续效果
        Instant, // 瞬时效果，只持续一场战斗，如技能
    }

    public enum LaunchTime
    {
        AllTime, // 总是生效（更改面板数据）
        BeforeBattle, // 战前生效
        AfterBattle, // 战后生效
    }


    public AdditionType type;
    public LaunchTime launchTime;

    /*
     * 对于玩家身上的Buff来说，unaryAddition的唯一参数就是player.property
     * binaryAddition的参数就是player.property，enemy.property
     * 对于怪物则相反
     */
    public UnaryAddition unaryAddition;
    public BinaryAddition binaryAddition;

    public Buff(AdditionType type, LaunchTime launchTime, UnaryAddition unaryAddition, BinaryAddition binaryAddition)
    {
        this.type = type;
        this.launchTime = launchTime;
        this.unaryAddition = unaryAddition;
        this.binaryAddition = binaryAddition;
    }

}