/*
 * file: NPC.cs
 * author: DeamonHook
 * feature: NPC
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : TriggerEntity
{
    protected string defaultType = "chat";
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();    
        this.passable = false;
        this.triggerType = defaultType;
        if (this.setting.ContainsKey("type"))
        {
            this.triggerType = setting["type"];
        }
    }

    public override void BeforeCollision()
    {
        base.BeforeCollision();
        GameManager.instance.TriggerByEntity(this);
    }
}
