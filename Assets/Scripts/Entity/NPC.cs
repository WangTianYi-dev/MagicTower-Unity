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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();    
        this.passable = false;
        if (!this.setting.ContainsKey("type"))
        {
            setting["type"] = "chat";
        }
    }

    public override void BeforeCollision()
    {
        base.BeforeCollision();
    }
}
