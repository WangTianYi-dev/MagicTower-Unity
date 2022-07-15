/*
 * file: Unit.cs
 * author: DeamonHook
 * date: 7/6/2022
 * feature: 所有单位的抽象基类
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Unit : MonoBehaviour
{
    /// <summary>
    /// 位置
    /// </summary>
    protected Vector2Int _position;
    public Vector2Int position { get { return _position; } }

    /// <summary>
    /// 所有单位
    /// </summary>
    protected State _state = State.Idle;

    /// <summary>
    /// Animator属性的引用
    /// </summary>
    protected Animator _animator;

    /// <summary>
    /// 物品词条
    /// </summary>
    protected Property _property;

    protected HashSet<string> _specials;

    public EventData[] eventDatas;
    public int eventDataId;
    public int eventIndex = 0;

    /// <summary>
    /// 单位的类型
    /// </summary>
    protected UnitType _type;
    public UnitType type { get => _type; set => _type = value; }

    /// <summary>
    /// 主角与单位碰撞前调用
    /// </summary>
    public virtual void BeforeMove() { }

    /// <summary>
    /// 主角与单位碰撞后调用
    /// </summary>
    public virtual void AfterMove() { }

    /// <summary>
    /// 创建单位
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="eventDataId"></param>
    /// <param name="cover">是否覆盖原有单位</param>
    public virtual void Create(Vector2Int pos, int eventDataId, bool cover)
    {
        this.eventDataId = eventDataId;
        //eventDatas = ResSvr.Instance.GetEventDatasById(eventDatasId);
        this._position = pos;

        if (cover)
        {
            //Tile tile = 
        }
    }
}
