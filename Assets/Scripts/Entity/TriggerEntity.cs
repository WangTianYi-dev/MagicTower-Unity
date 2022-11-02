/*
 * file: TriggerEntity.cs
 * author: DeamonHook
 * feature: 触发entity，如NPC和楼梯等
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 不要实例化该类，若需要，请实例化其子类，如NPC，TriggerArea
public abstract class TriggerEntity : Entity
{
    // 注意：setting的key均为小写
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
