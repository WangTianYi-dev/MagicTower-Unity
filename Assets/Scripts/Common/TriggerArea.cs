/*
 * file: TriggerData.cs
 * author: DeamonHook
 * feature: 触发数据
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class TriggerArea
{
    public Vector2Int position;

    public List<string> keys;
    public List<string> values;

    public TriggerArea()
    {
        this.keys = new List<string>();
        this.values = new List<string>();
    }

    public void Add(string key, string value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public TriggerArea(Vector2Int position, List<string> keys, List<string> values)
    {
        this.position = position;
        this.keys = keys;
        this.values = values;
    }

    public TriggerArea(TriggerArea area)
    {
        this.position = area.position;
        this.keys = new List<string>(area.keys);
        this.values = new List<string>(area.values);
    }

    public Dictionary<string, string> ToDict()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        for (int i = 0; i < keys.Count; i++)
        {
            dict.Add(keys[i], values[i]);
        }
        return dict;
    }

    
}