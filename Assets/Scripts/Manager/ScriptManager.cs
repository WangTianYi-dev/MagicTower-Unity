/*
 * file: ScriptManager.cs
 * author: DeamonHook
 * feature: Lua½Å±¾Ö§³Ö
 */

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public static class ScriptInterface
{
    public static void Log(string message)
    {
        Debug.Log(message);
    }

    public static void SetGlobal(string name, string value)
    {
        GameManager.instance.globalDict[name] = value;
    }

    public static bool VerifyGlobal(string name, string value)
    {
        var dict = GameManager.instance.globalDict;
        if (dict.ContainsKey(name) && dict[name] == value)
        {
            return true;
        }
        return false;
    }
}

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager instance { get; private set; }

    internal const float GCInterval = 1;
    internal float lastGCTime = 0;

    public Dictionary<string, string> globalDict
    {
        get
        {
            return GameManager.instance.globalDict;
        }
    }

    private LuaEnv luaEnv = new LuaEnv();

    private LuaTable scriptEnv;

    private void SetInterface()
    {
        var t = typeof(ScriptInterface);
        MethodInfo[] methods = t.GetMethods();
        foreach (MethodInfo method in methods)
        {

        }
    }

    private void Awake()
    {
        instance = this;
        scriptEnv = luaEnv.NewTable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastGCTime > GCInterval)
        {
            luaEnv.Tick();
            lastGCTime = Time.time;
        }
    }
}
