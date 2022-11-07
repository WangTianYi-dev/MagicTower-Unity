/*
 * file: Figure.cs
 * author: DeamonHook
 * feature: 怪物和玩家（战斗单位）的抽象基类
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Figure : Entity
{
    /// <summary>
    /// 属性
    /// </summary>
    public Property property;

    /// <summary>
    /// 瞬时技能（怪物只有瞬时技能）
    /// </summary>
    public BinaryAddition special;

    public string[] specials;

    [HideInInspector]
    /// <summary>
    /// 经过加成后的属性，显示在面板上，不用于伤害计算
    /// </summary>
    public Property externalProperty;

    public virtual void ApplySpecial() { }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        externalProperty = property;
    }

    public override void AfterMoveTo()
    {
        base.AfterMoveTo();
        KillSelf();
    }
}
