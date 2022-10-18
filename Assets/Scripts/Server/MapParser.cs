/*
 * file: MapParser.cs
 * author: DeamonHook
 * feature: 地图（xml）解析
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Xml.Schema;

public class MapParser
{

    private string mapName;


    private int width = 11, height = 11; // TODO:支持更多尺寸

    private XmlDocument doc = new XmlDocument();

    private XmlElement rootElem; //地图的xmlElement对象

    private List<int> ground = new List<int>(), unit = new List<int>();

    private Dictionary<int, string> nameDict = new Dictionary<int, string>(); //索引和名称的词典

    /// <summary>
    /// 读取tmx所引用的tileset
    /// </summary>
    /// <param name="name">路径</param>
    /// <param name="firstgid">tmx所规定的gid偏置量</param>
    private void LoadTileset(string name, int firstgid)
    {
        var t = AssetServer.GetAssetByPath<TextAsset>(Path.Combine("Maps", name));
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(t.text);
        var elem = doc.DocumentElement;
        var list = elem.SelectNodes("/tileset/tile");
        foreach (XmlNode tile in list)
        {
            if (tile.Attributes["class"] != null)
            {
                nameDict.Add(int.Parse(tile.Attributes["id"].Value) + firstgid, tile.Attributes["class"].Value);
            }
        }
    }

    private void LoadLayer(XmlNode node)
    {
        string name = node.Attributes["name"].Value;
        string csvText = node.InnerText;
        char[] delims = { ',', ' ', '\n', '\r' };
        var ss = csvText.Split(delims, StringSplitOptions.RemoveEmptyEntries);

        List<int> ptr = null;

        switch (name)
        {
            case "Ground" or "ground":
                ptr = ground;
                break;
            case "Unit" or "unit":
                ptr = unit;
                break;
            default:
                break;
        }

        if (ptr != null)
        {
            ptr.Clear();
            foreach (var s in ss)
            {

                ptr.Add(int.Parse(s));

            }
        }
    }

    private Dictionary<int, string> tmxList = new Dictionary<int, string>();

    private readonly int tileWidth = 32, tileHeight = 32;

    /// <summary>
    /// 读取Ground和Unit层（假定map已经被定义）
    /// </summary>
    private void ParseBasicLayers()
    {
        //目前版本锁定了地图尺寸和方向
        Assert.IsTrue(rootElem.GetAttribute("renderorder") == "right-down" && rootElem.GetAttribute("orientation") == "orthogonal" && rootElem.GetAttribute("width") == "11" && rootElem.GetAttribute("height") == "11");
        Assert.IsTrue(rootElem.GetAttribute("tilewidth") == "32" && rootElem.GetAttribute("tileheight") == "32");

        //tileset读取
        XmlNodeList tstList = rootElem.SelectNodes("./tileset");
        foreach (XmlNode n in tstList)
        {
            this.tmxList.TryAdd(int.Parse(n.Attributes["firstgid"].Value), n.Attributes["source"].Value.Split('.')[0]);
        }

        //读取tmx所引用的tsx
        nameDict = new Dictionary<int, string> { { 0, "" } };
        foreach (var v in this.tmxList)
        {
            LoadTileset(v.Value, v.Key);
        }

        XmlNodeList layerList = rootElem.SelectNodes("./layer");

        foreach (XmlNode layer in layerList)
        {
            LoadLayer(layer);
        }
    }

    /// <summary>
    /// 根据地图文件名（无后缀）获取xml文件内容
    /// </summary>
    /// <param name="name">文件名（无后缀）</param>
    /// <returns>tmx文件原始内容</returns>
    public TextAsset FetchRawMap(string name)
    {
        var path = Path.Combine("Maps", name);
        var t = AssetServer.GetAssetByPath<TextAsset>(path);
        return t;
    }

    public void FetchMapByName(string name)
    {
        TextAsset ast = FetchRawMap(name);
        mapName = ast.name;
        doc.LoadXml(ast.text);
        rootElem = doc.DocumentElement;
        ParseBasicLayers();
    }

    /// <summary>
    /// 将渲染顺序为right-down的元素的位置转换为游戏所用的right-up坐标
    /// </summary>
    /// <param name="innerRank">列表中元素的位置</param>
    /// <returns>right-up坐标</returns>
    private (int x, int y) TranCord(int innerRank)
    {
        int x = innerRank % width;
        int y = height - (innerRank / width) - 1;
        Assert.IsTrue(x >= 0 && x < width && y >= 0 && y < height);
        return (x, y);
    }

    /// <summary>
    /// 将渲染顺序为right-down的元素的位置转换为游戏所用的right-up坐标
    /// </summary>
    /// <param name="ox">原始x</param>
    /// <param name="oy">原始y</param>
    /// <returns></returns>
    private Vector2Int TranCord(int ox, int oy)
    {
        int x = ox;
        int y = height - 1 - oy;
        return new Vector2Int(x, y);
    }


    /// <summary>
    /// 将tmx内部的列表转换为string二维数组
    /// </summary>
    /// <param name="innerLayer"></param>
    /// <returns>string二维数组</returns>
    private string[,] TranLayer(List<int> innerLayer)
    {
        string[,] g = new string[width, height];
        for (int i = 0; i < innerLayer.Count; i++)
        {
            (int x, int y) = TranCord(i);
            g[x, y] = nameDict[innerLayer[i]];
        }
        return g;
    }

    /// <summary>
    /// 获取ground层
    /// </summary>
    /// <returns>ground层的二维string</returns>
    public string[,] GetGround()
    {
        return TranLayer(ground);
    }

    /// <summary>
    /// 获取unit层
    /// </summary>
    /// <returns>ground层的二维string</returns>
    public string[,] GetUnit()
    {
        return TranLayer(unit);
    }

    public Dictionary<Vector2Int, List<KeyValuePair<string, string>>> GetSetting()
    {
        var setting = new Dictionary<Vector2Int, List<KeyValuePair<string, string>>>();

        var node = rootElem.SelectSingleNode("./objectgroup[@name='Setting']");
        var lst = node.SelectNodes("./object");

        foreach (XmlNode r in lst)
        {
            float x = float.Parse(r.Attributes["x"].Value);
            float y = float.Parse(r.Attributes["y"].Value);
            int ix = (int)(x / tileWidth);
            int iy = (int)(y / tileHeight);
            Vector2Int v = TranCord(ix, iy);

            var content = new List<KeyValuePair<string, string>>();
            var prptyLst = r.SelectNodes("./properties/property");

            foreach (XmlNode prpty in prptyLst)
            {
                var name = prpty.Attributes["name"].Value;
                var value =
                    prpty.Attributes["value"] == null ?
                    prpty.InnerText :
                    prpty.Attributes["value"].Value;
                content.Add
                (
                    new KeyValuePair<string, string>(name, value)
                );
            }
            setting[v] = content;
        }

        return setting;
    }


    public List<TriggerArea> GetTriggers()
    {
        var triggers = new List<TriggerArea>();
        //var setting = new Dictionary<Vector2Int, List<KeyValuePair<string, string>>>();

        var node = rootElem.SelectSingleNode("./objectgroup[@name='Trigger']");
        var lst = node.SelectNodes("./object");

        foreach (XmlNode r in lst)
        {
            float x = float.Parse(r.Attributes["x"].Value);
            float y = float.Parse(r.Attributes["y"].Value);
            int ix = (int)(x / tileWidth);
            int iy = (int)(y / tileHeight);
            Vector2Int v = TranCord(ix, iy);

            var content = new TriggerArea();
            content.position = v;

            var prptyLst = r.SelectNodes("./properties/property");

            foreach (XmlNode prpty in prptyLst)
            {
                var name = prpty.Attributes["name"].Value;
                var value =
                    prpty.Attributes["value"] == null ?
                    prpty.InnerText :
                    prpty.Attributes["value"].Value;
                content.Add(name, value);
            }
            triggers.Add(content);
        }

        return triggers;
    }
    

    /// <summary>
    /// 将已经加载的xml地图转化为tilemap
    /// </summary>
    /// <returns></returns>
    public Tilemap GetTilemap()
    {
        Tilemap tilemap = new Tilemap();
        tilemap.mapHeight = height;
        tilemap.mapWidth = width;
        tilemap.mapName = mapName;
        tilemap.unit = GetUnit();
        tilemap.ground = GetGround();
        tilemap.setting = GetSetting();
        tilemap.triggerAreas = GetTriggers();
        return tilemap;
    }

}
