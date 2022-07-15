/*
 * file: Tile.cs
 * author: DeamonHook
 * date: 7/6/2022
 * feature: 瓦片系统 (瓦片是组成地图的基本单元, 包括单位层和地面层)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// 是否可通行（无单位时）
    /// </summary>
    public bool passable;
    /// <summary>
    /// 位置
    /// </summary>
    private Vector2Int position;
    /// <summary>
    /// 对格子上单位的引用
    /// </summary>
    public Unit unit = null;

    public bool canMove
    {
        get
        {
            if (!passable) return false;
            if (unit == null) return true;
            switch (unit.type)
            {
                case UnitType.Enemy:
                case UnitType.Door:
                case UnitType.NPC:
                case UnitType.Player:
                    return false;
                case UnitType.Item:
                case UnitType.Stair:
                case UnitType.EventPoint:
                    return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    private void OnMouseDown()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;
        //GameManager.Instance.FindPathPlayer(this);

    }

    /// <summary>
    /// 主角移动到该瓦片前的委托
    /// </summary>
    public Action actBeforeMove;

    /// <summary>
    /// 主角移动到该瓦片后的委托
    /// </summary>
    public Action actAfterMove;

    /// <summary>
    /// 移动前调用
    /// </summary>
    public void BeforeMove()
    {
        actBeforeMove?.Invoke();
        if (unit != null) unit.BeforeMove();
    }
    /// <summary>
    /// 移动后调用
    /// </summary>
    public void AfterMove()
    {
        actAfterMove?.Invoke();
        if (unit != null) unit.AfterMove();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
