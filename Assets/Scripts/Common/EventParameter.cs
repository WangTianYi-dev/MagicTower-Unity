/*
 * file: EventParameter.cs
 * author: DeamonHook
 * feature: 事件所传递的参数
 */

using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 事件参数
/// </summary>
public class EventParameter: EventArgs
{
    public EventType eventType;
    public Dictionary<string, string> parameters;
}