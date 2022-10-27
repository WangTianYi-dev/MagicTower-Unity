/*
 * file: Archive.cs
 * author: DeamonHook
 * feature: 游戏存档
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// 最终被转换成json的对象
[Serializable]
public class Archive
{
    public int version; // 将来扩展用
    public List<MapArchive> mapArchive; // Map
    public Property playerProperty;
    public List<string> playerSkills;
    public List<ItemPair> playerItems;
    public List<string> playerEquipments;
    public string curMap;
    public Vector2Int curPos;
    public int altarCount; // 祭坛购买次数
    public List<TriggerArea> triggerAreas;
    // 创建Archive的方法在GameManager中

    public List<string> globalKeys;
    public List<string> globalValues;
}


[Serializable]
public class ItemPair
{
    public string name;
    public int count;
}


[Serializable]
public class MapArchive
{
    public string name;
    public int width, height;
    public List<string> ground, unit;
    public List<SettingArchive> settings;
    public List<TriggerArea> triggerAreas;
    public Vector2Int lastPos;
    
    public MapArchive(Tilemap tilemap)
    {
        name = tilemap.mapName;
        width = tilemap.mapWidth;
        height = tilemap.mapHeight;
        lastPos = tilemap.lastPos;
        ground = new List<string>();
        unit = new List<string>();
        settings = new List<SettingArchive>();
        triggerAreas = new List<TriggerArea>(tilemap.triggerAreas);
        foreach (var g in tilemap.ground)
        {
            ground.Add(g);
        }
        foreach (var u in tilemap.unit)
        {
            unit.Add(u);
        }
        foreach (var s in tilemap.setting)
        {
            settings.Add(new SettingArchive(s));
        }
    }

    private string[,] ConvertTo2Dim(List<string> l)
    {
        var g = new string[width, height];
        for (int i = 0; i < l.Count; i++)
        {
            int r = i / width, c = i % width;
            g[r, c] = l[i];
        }
        return g;
    }

    public Tilemap ToTilemap()
    {
        Tilemap tilemap = new Tilemap();
        tilemap.mapName = name;
        tilemap.mapWidth = width;
        tilemap.mapHeight = height;
        tilemap.lastPos = lastPos;
        tilemap.unit = ConvertTo2Dim(unit);
        tilemap.ground = ConvertTo2Dim(ground);
        tilemap.triggerAreas = new List<TriggerArea>(triggerAreas);
        tilemap.setting = new Dictionary<Vector2Int, List<KeyValuePair<string, string>>>();
        foreach (var p in settings)
        {
            tilemap.setting.Add(new Vector2Int(p.x, p.y), p.ToSettingValue());
        }
        return tilemap;
    }
}

[Serializable]
public class SettingArchive
{
    public int x, y;
    public List<string> keys;
    public List<string> values;

    // 将一个单独的设置转化为存档
    public SettingArchive(KeyValuePair<Vector2Int, List<KeyValuePair<string, string>>> setting)
    {
        x = setting.Key.x;
        y = setting.Key.y;
        keys = new List<string>();
        values = new List<string>();
        foreach (var p in setting.Value)
        {
            keys.Add(p.Key);
            values.Add(p.Value);
        }
    }

    public List<KeyValuePair<string, string>> ToSettingValue()
    {
        var l = new List<KeyValuePair<string, string>>();
        for (int i = 0; i < keys.Count; i++)
        {
            l.Add(new KeyValuePair<string, string>(keys[i], values[i]));
        }
        return l;
    }
}

[Serializable]
public class MetaArchive
{
    public int version;
    public int index;
    public string areaName, HP, ATK, DEF, Coin;
}