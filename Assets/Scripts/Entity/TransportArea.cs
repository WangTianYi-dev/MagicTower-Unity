/*
 * file: TransportArea
 * .cs
 * author: DeamonHook
 * feature: 触发区域，如NPC和楼梯等
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportArea : NPC
{

    protected override void Start()
    {
        base.Start();
        this.passable = true;
        setting["type"] = "transport";
    }

    public override void BeforeCollision()
    {
        base.BeforeCollision();
    }
}
