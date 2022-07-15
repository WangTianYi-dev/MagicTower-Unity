/*
 * file: MapManager.cs
 * author: DeamonHook
 * date: 7/7/2022
 * feature: 地图管理器
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : BaseManager
{
    public static MapManager instance { get; private set; }

    private Transform groundTran;
    private LineRenderer lineRenderer;
    [HideInInspector]
    public Transform unitTran;
    [HideInInspector]
    public int floor;

    /// <summary>
    /// upPos: 上楼时初始位置, downPos: 下楼时初始位置
    /// </summary>
    public Vector2Int upPos, downPos;

    /// <summary>
    /// mapCol: 地图宽度, mapRow: 地图高度
    /// </summary>
    public int mapCol = 13, mapRow = 13;

    /// <summary>
    /// 瓦片存放处
    /// </summary>
    private Tile[,] tiles;


    public override void Init()
    {
        base.Init();
        instance = this;
        groundTran = GameObject.Find("Ground").transform;
        unitTran = GameObject.Find("Unit").transform;
        lineRenderer = unitTran.GetComponent<LineRenderer>();
        tiles = new Tile[mapCol, mapRow];
    }

    /// <summary>
    /// 清空地图(摧毁所有瓦片和其上的对象)
    /// </summary>
    public void Clear()
    {
        if (tiles[0, 0] != null)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.unit != null)
                {
                    Destroy(tile.unit.gameObject);
                }
                Destroy(tile.gameObject);
            }
        }
    }

    public void LoadMap(int id)
    {
        //todo
    }

    private void LoadUnits()
    {
        //todo
    }

    //外部接口
    public Tile GetTileByPos(Vector2Int pos)
    {
        return tiles[pos.x, pos.y];
    }

    public Unit GetUnitByPos(Vector2Int pos)
    {
        return tiles[pos.x, pos.y].unit;
    }


}
