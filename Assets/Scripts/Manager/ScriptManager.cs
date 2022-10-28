/*
 * file: ScriptManager.cs
 * author: DeamonHook
 * feature: Lua脚本支持
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

    private void SetInterface()
    {
        var t = typeof(ScriptInterface);
        MethodInfo[] methods = t.GetMethods();
        foreach (MethodInfo method in methods)
        {
            if (method.DeclaringType == t)
            {
                luaEnv.DoString($"{method.Name} = CS.ScriptInterface.{method.Name}");
            }
        }
    }

    private void Awake()
    {
        instance = this;
        SetInterface();
    }

    public void DoString(string str)
    {
        LuaTable env = luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        env.SetMetaTable(meta);
        meta.Dispose();

        env.Set("self", this);
        luaEnv.DoString(str, "chunk", env);
    }

    private void Start()
    {
        DoString("Log(\"Lua says Hello!\")");
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
