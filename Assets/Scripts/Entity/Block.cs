/*
 * file: Block.cs
 * author: DeamonHook
 * feature: 障碍物，如门
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    [Header("消除障碍的条件，如钥匙等")]
    public string requirement;

    [Header("被阻挡时的信息")]
    public string blockedMessage;

    protected override void Start()
    {
        base.Start();
        this.passable = false;
    }

    public override void BeforeCollision()
    {
        base.BeforeCollision();
        if (GameManager.instance.AskRequirement(requirement))
        {
            this.passable = true;
        }
    }

    public override void AfterBlocked()
    {
        base.AfterBlocked();
        UIManager.instance.PopMessage(blockedMessage);
    }

    public override void AfterMoveTo()
    {
        base.AfterMoveTo();
        DestroyBlock();
    }
    


    public void DestroyBlock()
    {
        Destroy(gameObject);
    }
}
