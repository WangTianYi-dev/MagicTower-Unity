/*
 * file: BaseTrigger.cs
 * author: DeamonHook
 * feature: 触发的基类
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

class Trigger
{
    public Trigger(string parameter)
    { }
    
    public static Trigger CreateTrigger(string type, string parameter)
    {
        Type t = System.Type.GetType(type, false, true);
        if (t == null || !t.IsSubclassOf(typeof(Trigger)))
        {
            Debug.LogError($"不支持的触发类型: {type}");
            return null;
        }
        return Activator.CreateInstance(t, parameter) as Trigger;
    }

    /// <summary>
    /// 开启触发
    /// </summary>
    public virtual void Fire() { }
}