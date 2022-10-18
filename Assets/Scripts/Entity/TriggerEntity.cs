/*
 * file: TriggerArea.cs
 * author: DeamonHook
 * feature: ����������NPC��¥�ݵ�
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ҫʵ�������࣬����Ҫ����ʵ���������࣬��NPC��TriggerArea
public abstract class TriggerEntity : Entity
{
    // ע�⣺setting��key��ΪСд
    public Dictionary<string, string> setting {get; private set;}

    [Header("Ĭ�ϴ�������")]
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
