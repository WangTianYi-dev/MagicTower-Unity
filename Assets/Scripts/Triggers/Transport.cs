

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

class Transport : Trigger
{
    /// <summary>
    /// 传送参数须遵守的正则
    /// </summary>
    static Regex transportRegex = new Regex(@"\s*(?<map>[^,]+?)\s*,\s*(?<x>[0-9]+)\s*,\s*(?<y>[0-9]+)\s*]\s*");

    string map;
    int x, y;

    public Transport(string parameter) : base(parameter)
    {
        var match = transportRegex.Match(parameter);
        if (!match.Success)
        {
            Debug.LogError($"{parameter} 不是有效的Transport参数");
        }
    }

    public override void Fire()
    {
        base.Fire();

    }
}