/*
 * file: Entity.cs
 * author: DeamonHook
 * feature: 实体的抽象基类
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 游戏中每个图层都由实体组成
/// </summary>
public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// 逻辑位置
    /// </summary>
    public Vector2Int logicPos { get; set; }

    
    /// <summary>
    /// 当前状态
    /// </summary>
    protected State state = State.Idle;


    [Header("游戏内名字")]
    public string nameInGame;

    [Header("是否可通过")]
    public bool passable = false;

    [Header("碰撞前生成的SpriteEffect物体")]
    public GameObject effectBeforeCollision;
    
    [Header("移动后生成的SpriteEffect物体")]
    public GameObject effectAfterKilled;

    public EntityType type { get; protected set; }


    protected virtual void Start()
    {
        this.logicPos = Util.GetPosition(transform);

    }

    protected virtual void Awake() { }

    /// <summary>
    /// 玩家将要碰撞之前调用
    /// </summary>
    public virtual void BeforeCollision() 
    {
        if (effectBeforeCollision != null)
        {
            EffectLayer.instance.CreateSpriteEffect(logicPos, effectBeforeCollision);
        }
    }

    /// <summary>
    /// 玩家移动到实体上之后触发的事件
    /// </summary>
    public virtual void AfterMoveTo() 
    {
    }

    public virtual void AfterKilled()
    {
        if (effectAfterKilled != null)
        {
            EffectLayer.instance.CreateSpriteEffect(logicPos, effectAfterKilled);
        }
    }

    /// <summary>
    /// 玩家被阻挡后触发的事件
    /// </summary>
    public virtual void AfterBlocked() { }

    public void DestroySelf()
    {
        MapManager.instance.KillEntity(this);
    }

    public bool IsSame(Entity another)
    {
        return nameInGame == another.nameInGame;
    }

    public void KillSelf()
    {
        GameManager.instance.RemoveEntity(this);
    }
}


