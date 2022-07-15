/*
 * file: ResourceResolver.cs
 * author: DeamonHook
 * date: 7/6/2022
 * feature: 数据解析
 */

using LightJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class ResResolver : MonoBehaviour
{
    public static ResResolver instance;

    public void Init()
    {
        instance = this;
    }

    private Dictionary<int, GroundData> groundDic = new Dictionary<int, GroundData>();
    public void InitMapDic(string path, bool isNew = true)
    {
        JsonArray groundData;
        if (isNew)
        {
            groundData = JsonValue.Parse(Resources.Load<TextAsset>(path).text);
        }
        else
        {
            groundData = JsonValue.Parse(File.ReadAllText(path));
        }
        foreach (JsonValue md in groundData)
        {
            GroundData ground = new GroundData
            {
                floor = md["floor"],
                data = md["data"]
            };
            groundDic.Add(ground.floor, ground);
        }
    }

    public GroundData GetMapByFloor(int floor)
    {
        GroundData map;
        if (groundDic.TryGetValue(floor, out map))
        {
            return map;
        }
        return null;
    }
}

