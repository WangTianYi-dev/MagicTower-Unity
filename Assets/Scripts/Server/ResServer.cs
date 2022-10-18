/*
 * file: ResServer.cs
 * author: DeamonHook
 * feature: 资源服务器
 */

//using LightJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class ResServer : MonoBehaviour
{
    public static ResServer instance { get; private set; }

    private MapParser mapParser;

    private void Awake()
    {
        instance = this;
        mapParser = new MapParser();
        InitPrefabDict();
        InitEntityNameDict();
    }

    // 键：prefab名字，值：prefab 
    private Dictionary<string, GameObject> prefabDict = 
        new Dictionary<string, GameObject>();

    // 键：Entity.nameInGame，值：entity的名字
    private Dictionary<string, string> entityNameDict = 
        new Dictionary<string, string>();

    /// <summary>
    /// 初始化预制件词典（所有键强制为小写）
    /// </summary>
    private void InitPrefabDict()
    {
        //Debug.Log("InitPrefabDict");
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach (var prefab in prefabs)
        {
            prefabDict[prefab.name.ToLower()] = prefab;
        }
    }

    private void InitEntityNameDict()
    {
        var prefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach (var prefab in prefabs)
        {
            if (prefab.GetComponent<Entity>() != null)
            {
                entityNameDict.Add(prefab.GetComponent<Entity>().nameInGame, prefab.name);
            }
        }
    }

    /// <summary>
    /// 根据名称获取对应的prefab
    /// </summary>
    /// <param name="name">名称（大小写不敏感）</param>
    /// <returns>prefab，若当前prefab词典中不存在则返回null</returns>
    public GameObject GetObject(string name)
    {
        if (prefabDict.ContainsKey(name.ToLower()))
        {
            return prefabDict[name.ToLower()];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 根据名称获取prefab的sprite
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetEntitySprite(string name)
    {
        if (prefabDict.ContainsKey(name.ToLower()))
        {
            return prefabDict[name.ToLower()].GetComponent<SpriteRenderer>().sprite;
        }
        else return null;
    }

    public T GetEntityComponent<T>(string name)
    {
        var prefab = prefabDict[name.ToLower()];
        return prefab.GetComponent<T>();
    }

    /// <summary>
    /// 根据名称获得对应的MapData
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>MapData</returns>
    public Tilemap GetTilemap(string name)
    {
        mapParser.FetchMapByName(name);
        var tm = mapParser.GetTilemap();
        return tm;
    }

    /// <summary>
    /// 根据entity的nameInGame属性获取其内部名称
    /// </summary>
    /// <param name="e">entity</param>
    /// <returns>内部名称</returns>
    public string GetEntityName(Entity e)
    {
        return entityNameDict[e.nameInGame];
    }
}

