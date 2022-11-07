/*
 * file: Block.cs
 * author: DeamonHook
 * feature: �ϰ������
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    [Header("�����ϰ�����������Կ�׵�")]
    public string requirement;

    [Header("���赲ʱ����Ϣ")]
    public string blockedMessage;

    protected override void Start()
    {
        base.Start();
        this.passable = false;
        if (blockedMessage == "")
        {
            blockedMessage = "�޷�ͨ��";
        }
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

    public override void OnKilled()
    {
        base.OnKilled();
    }

    public override void AfterMoveTo()
    {
        base.AfterMoveTo();
        KillSelf();
    }
}
