using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using static Buff;

public class Skill : Entity
{
    [Header("收集后的提示信息")]
    public string messageAfterCollect;

    [Header("内部名称（与prefab名称一致）")]
    public string internalName;

    protected override void Start()
    {
        base.Start();
        if (internalName == "")
        {
            internalName = ResServer.instance.GetEntityName(this);
        }
        internalName = internalName.ToLower();
        this.type = EntityType.Skill;
        this.passable = true;
    }


    public override void AfterMoveTo()
    {
        base.AfterMoveTo();
        Player.instance.skills.Add(internalName);
        UIManager.instance.PopMessage(messageAfterCollect);
        GameManager.instance.RemoveEntity(this);
    }

    // 介绍信息，key为小写
    public static Dictionary<string, string> SkillIntro = new Dictionary<string, string>
    {
        {
            "fangu",
            "剑技：凡骨（等级1）\n消耗：50点生命  效果：攻击+10，防御—10\n勇者在战斗中领悟的技能\n平庸的凡人，如果坚定决死的意志，也能灿烂地绽放"
        },
    };

    /// <summary>
    /// 技能使用的前提条件，返回true则成功使用，返回false则使用失败
    /// </summary>
    public static Dictionary<string, Func<bool>> prerequsiteDict = new Dictionary<string, Func<bool>>
    {
        {
            "fangu",
            () =>
            {
                if (Player.instance.property.HP <= 50)
                {
                    return false;
                }
                Player.instance.property.HP -= 50;
                return true;
            }
        }
    };

    public static Dictionary<string, List<Buff>> SkillBuffDict = new Dictionary<string, List<Buff>>
    {
        {
            "fangu",
            new List<Buff> {
                new Buff(
                    AdditionType.Instant,
                    LaunchTime.AllTime,
                    new UnaryAddition(
                        (ref Property self) =>
                        {
                            self.ATK += 10;
                            self.DEF -= 10;
                        }
                    ),
                    null
                )
            }
        }
    };
}
