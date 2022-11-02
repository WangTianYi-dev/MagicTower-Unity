/*
 * file: TriggerEntity.cs
 * author: DeamonHook
 * feature: ����entity����NPC��¥�ݵ�
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ҫʵ�������࣬����Ҫ����ʵ���������࣬��NPC��TriggerArea
public abstract class TriggerEntity : Entity
{
    // ע�⣺setting��key��ΪСд
    public Dictionary<string, string> setting;

    protected override void Start()
    {
        base.Start();
        if (setting == null)
        {
            setting = GameManager.instance.RequestSetting(logicPos);
        }
    }

    public virtual void OnTriggerDone()
    {
        if (this.setting.ContainsKey("killafterdone") || this.setting.ContainsKey("dieafterdone"))
        {
            GameManager.instance.RemoveEntity(this);
        }
    }

    public override void BeforeCollision()
    {
        base.BeforeCollision();
        GameManager.instance.TriggerByEntity(this);
    }
}
