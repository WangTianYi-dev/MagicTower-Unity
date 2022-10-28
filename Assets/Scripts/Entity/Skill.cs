using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using static Buff;

public class Skill : Entity
{
    [Header("�ռ������ʾ��Ϣ")]
    public string messageAfterCollect;

    [Header("�ڲ����ƣ���prefab����һ�£�")]
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

    // ������Ϣ��keyΪСд
    public static Dictionary<string, string> SkillIntro = new Dictionary<string, string>
    {
        {
            "fangu",
            "���������ǣ��ȼ�1��\n���ģ�50������  Ч��������+10��������10\n������ս��������ļ���\nƽӹ�ķ��ˣ�����ᶨ��������־��Ҳ�ܲ��õ�����"
        },
    };

    /// <summary>
    /// ����ʹ�õ�ǰ������������true��ɹ�ʹ�ã�����false��ʹ��ʧ��
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