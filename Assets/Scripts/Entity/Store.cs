/*
 * file: Store.cs
 * author: DeamonHook
 * feature: 不会涨价的商店
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class Store : NPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 
        this.passable = false;
        this.triggerType = this.triggerType == "" ? "trade" : this.triggerType;
    }


    public override void BeforeCollision()
    {
        base.BeforeCollision();
        GameManager.instance.TriggerByEntity(this);
    }
}
