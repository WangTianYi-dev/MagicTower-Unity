using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Entity
{
    public string blockedMessage = "无法通行";

    protected override void Awake()
    {
        base.Awake();
        this.type = EntityType.Ground;
    }

    protected override void Start()
    {
        base.Start();
        this.passable = false;
    }

    public override void AfterBlocked()
    {
        base.AfterBlocked();
        UIManager.instance.PopMessage(blockedMessage);
    }
}

