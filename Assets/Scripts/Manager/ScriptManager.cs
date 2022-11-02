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
using System;

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

    public static GameObject CreateUnitEntity(string name, int x, int y)
    {
        Debug.Log($"Creating {name}");
        name = name.ToLower();
        Vector2Int pos = new Vector2Int(x, y);
        return MapManager.instance.ReplaceUnitEntity(pos, name);
    }

    public static GameObject CreateGroundEntity(string name, int x, int y)
    {
        name = name.ToLower();
        Vector2Int pos = new Vector2Int(x, y);
        return MapManager.instance.ReplaceGroundEntity(pos, name);
    }

    public static bool KillUnitEntity(int x, int y)
    {
        Debug.Log($"KillUnitEntity at {x}, {y}");
        Entity e;
        MapManager.instance.unitEntityDict.TryGetValue(new Vector2Int(x, y), out e);
        if (e != null)
        {
            Debug.Log($"Kill {e}");
            MapManager.instance.KillEntity(e);
            return true;
        }
        return false;
    }

    public static bool KillGroundEntity(int x, int y)
    {
        Entity e;
        MapManager.instance.groundEntityDict.TryGetValue(new Vector2Int(x, y), out e);
        if (e != null)
        {
            MapManager.instance.KillEntity(e);
            return true;
        }
        return false;
    }

    public static void AddSetting(string key, string value, int x, int y)
    {
        MapManager.instance.AddSetting(new Vector2Int(x, y), key.ToLower(), value);
    }

    public static string GetUnitEntityName(int x, int y)
    {
        return MapManager.instance.curTilemap.unit[x, y];
    }

    public static string GetGroundEntityName(int x, int y)
    {
        return MapManager.instance.curTilemap.ground[x, y];
    }

    public static void PlayerSuspend(float time)
    {
        GameManager.instance.PlayerStop();
        GameManager.instance.PlayerSuspend(time);
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
        GameManager.instance.AddEvent(() => {
            LuaTable env = luaEnv.NewTable();
            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            env.SetMetaTable(meta);
            meta.Dispose();
            env.Set("self", this);
            luaEnv.DoString(str, "chunk", env);
            GameManager.instance.CurrentEventDone();
        });

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
