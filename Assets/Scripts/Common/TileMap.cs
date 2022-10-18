/*
 * file: TileMap.cs
 * author: DeamonHook
 * feature: 瓦片地图，地图的内部存储方式
 */

using System;
using System.Collections.Generic;
using UnityEngine;


public class Tilemap
{
    public string mapName;
    public int mapWidth, mapHeight;
    public string[,] unit, ground;
    public Vector2Int lastPos;
    // 商店、老人等属性设置
    public Dictionary<Vector2Int, List<KeyValuePair<string, string>>> setting = new Dictionary<Vector2Int, List<KeyValuePair<string, string>>>();
    public List<TriggerArea> triggerAreas;
}

