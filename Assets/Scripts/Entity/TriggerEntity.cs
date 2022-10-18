/*
 * file: TriggerArea.cs
 * author: DeamonHook
 * feature: 触发区域，如NPC和楼梯等
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 不要实例化该类，若需要，请实例化其子类，如NPC，TriggerArea
public abstract class TriggerEntity : Entity
{
    // 注意：setting的key均为小写
    public Dictionary<string, string> setting {get; private set;}

    [Header("默认触发类型")]
    public string triggerType;

    protected override void Start()
    {
        base.Start();
        setting = GameManager.instance.RequestSetting(position);
        if (setting.ContainsKey("type"))
        {
            this.triggerType = setting["type"];
        }
    }

    public virtual void OnTriggerDone()
    {
        if (this.setting.ContainsKey("killafterdone") || this.setting.ContainsKey("dieafterdone"))
        {
            GameManager.instance.RemoveEntity(this);
        }
    }
}
