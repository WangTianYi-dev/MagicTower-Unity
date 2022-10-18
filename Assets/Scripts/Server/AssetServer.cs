/*
 * file: AssetServer.cs
 * author: DeamonHook
 * feature: 资源加载
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class AssetServer
{
    

    public static T[] LoadAllTextAssets<T>() where T: Object
    {
        var l = Resources.FindObjectsOfTypeAll<T>();
        return l;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">路径（无后缀）</param>
    /// <returns></returns>
    public static T GetAssetByPath<T>(string path) where T: Object
    {
        T t = Resources.Load<T>(path);
        if (t == null)
        {
            Debug.LogError($"Failed to Get Asset with path {path} and type {typeof(T)}");
        }
        return t;
    }
    // TODO 添加对AssetBundle的支持 
}

