/*
 * file: MapManager.cs
 * author: DeamonHook
 * feature: 地图管理器
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : BaseManager
{
    public static MapManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private Transform groundTran;

    [HideInInspector]
    public Transform unitTran;

    [HideInInspector]
    public int floor;

    // 与GameManager中的name2Tilemap指向同一对象
    public Dictionary<string, Tilemap> tilemapCache;
    public Tilemap curTilemap;


    /// <summary>
    /// 通过坐标访问ground层覆盖物
    /// </summary>
    public Dictionary<Vector2Int, Entity> groundEntityDict 
    {
        get
        {
            return GroundLayer.instance.entityDict;
        }
    }

    public HashSet<string> mapnameList = new HashSet<string>();

    /// <summary>
    /// 通过坐标访问Unit层覆盖物
    /// </summary>
    public Dictionary<Vector2Int, Entity> unitEntityDict
    {
        get
        {
            return UnitLayer.instance.entityDict;
        }
    }

    /// <summary>
    /// mapWidth: 地图宽度, mapHeight: 地图高度
    /// </summary>
    public int mapWidth
    {
        get
        {
            return curTilemap.mapWidth;
        }
    }

    public int mapHeight
    {
        get
        {
            return curTilemap.mapHeight;
        }
    }

    public Vector2Int boxSize
    {
        get
        {
            return new Vector2Int(mapWidth, mapHeight);
        }
    }

    [HideInInspector]
    public GroundLayer groundLayer;
    [HideInInspector]
    public UnitLayer unitLayer;


    public override void Init()
    {
        base.Init();
        instance = this;
        tilemapCache = new Dictionary<string, Tilemap>();
        groundTran = transform.Find("Ground").transform;
        groundLayer = groundTran.GetComponent<GroundLayer>();
        unitTran = transform.Find("Unit").transform;
        unitLayer = unitTran.GetComponent<UnitLayer>();
    }

    private bool GroundPassable(Vector2Int cord)
    {
        if (groundEntityDict.ContainsKey(cord))
        {
            return groundEntityDict[cord].passable;
        }
        return true;
    }

    private bool UnitPassable(Vector2Int cord)
    {
        if (unitEntityDict.ContainsKey(cord))
        {
            return unitEntityDict[cord].passable;
        }
        return true;
    }

    public bool PosInMap(Vector2Int pos)
    {
        return Util.Inside(
            pos, new Vector2Int(0, 0), new Vector2Int(mapWidth, mapHeight)
        );
    }

    // 检查坐标对应块是否可通行
    public bool BlockPassable(Vector2Int cord)
    {
        return GroundPassable(cord) && UnitPassable(cord);
    }

    public Tilemap LoadNewMap(string name)
    {
        Tilemap map = ResServer.instance.GetTilemap(name);
        return map;
    }

    private void AddMap(Tilemap map)
    {
        string name = map.mapName;
        mapnameList.Add(name);
        tilemapCache.Add(name, map);
    }

    public void ChangeMap(string name)
    {
        print($"changeMap: {name}");
        if (curTilemap != null)
        {
            curTilemap.lastPos = Player.instance.logicPos;
            tilemapCache[curTilemap.mapName] = curTilemap;
            if (curTilemap.mapSetting.ContainsKey("onleave"))
            {
                ScriptManager.instance.DoString(curTilemap.mapSetting["onleave"]);
            }
        }
        if (tilemapCache.ContainsKey(name))
        {
            //print("countains ");
            curTilemap = tilemapCache[name];
        }
        else
        {
            //print("loadnew");
            curTilemap = LoadNewMap(name);
            AddMap(curTilemap);
        }
        RefreshEntities(curTilemap);
        UIManager.instance.RefreshAreaName(name);
        RefreshTriggerAreas();
        if (curTilemap.mapSetting.ContainsKey("onenter"))
        {
            ScriptManager.instance.DoString(curTilemap.mapSetting["onenter"]);
        }
    }

    public void RefreshEntities(Tilemap tilemap)
    {
        groundEntityDict.Clear();
        unitEntityDict.Clear();
        GroundLayer.instance.Refresh(tilemap);
        UnitLayer.instance.Refresh(tilemap);
    }



    #region 实体（entity）处理
    /// <summary>
    /// 将e从当前的tilemap中移除并摧毁其gameobject
    /// </summary>
    /// <param name="e"></param>
    public void KillEntity(Entity e)
    {
        if (e == null) return;
        if (unitEntityDict.ContainsKey(e.logicPos) && unitEntityDict[e.logicPos] == e)
        {
            if (curTilemap.setting.ContainsKey(e.logicPos))
            {
                if (curTilemap.setting[e.logicPos].ContainsKey("onkilled"))
                {
                    string onKilled = curTilemap.setting[e.logicPos]["onkilled"];
                    GameManager.instance.AddEvent(() =>
                        {
                            ScriptManager.instance.DoString(onKilled);
                            GameManager.instance.CurrentEventDone();
                        }
                        );
                }
                curTilemap.setting.Remove(e.logicPos);
            }
            unitEntityDict[e.logicPos].OnKilled();
            unitEntityDict.Remove(e.logicPos);
            curTilemap.unit[e.logicPos.x, e.logicPos.y] = "";
            Destroy(e.gameObject);
        }
        else if (groundEntityDict.ContainsKey(e.logicPos) && groundEntityDict[e.logicPos] == e)
        {
            groundEntityDict[e.logicPos].OnKilled();
            groundEntityDict.Remove(e.logicPos);
            curTilemap.ground[e.logicPos.x, e.logicPos.y] = "";
            Destroy(e.gameObject);
        }
        else
        {
            Debug.LogError(e + " not found!");
        }
    }

    public bool[,] GetPassableGrid()
    {
        var grid = new bool[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                grid[x, y] = BlockPassable(new Vector2Int(x, y));
            }
        }
        return grid;
    }

    /// <summary>
    /// 获取setting层的内容
    /// </summary>
    /// <param name="pos">坐标</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSettingAtPos(Vector2Int pos)
    {
        Dictionary<string, string> setting = new Dictionary<string, string>();
        if (curTilemap.setting.ContainsKey(pos))
        {
            foreach (var p in curTilemap.setting[pos])
            {
                setting.Add(p.Key.ToLower(), p.Value);
            }
        }
        return setting;
    }

    /// <summary>
    /// 替换ground层特定位置上的实体（不调用OnKilled）
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="name">替换者的内部名字</param>
    public GameObject ReplaceGroundEntity(Vector2Int pos, string name)
    {
        print($"ReplaceGroundEntity name {name} at {pos}");
        if (groundEntityDict.ContainsKey(pos))
        {
            GameObject go = groundEntityDict[pos].gameObject;
            groundEntityDict.Remove(pos);
            curTilemap.ground[pos.x, pos.y] = "";
            Destroy(go);
        }
        curTilemap.ground[pos.x, pos.y] = name.ToLower();
        return groundLayer.AddEntity(name, pos);
    }

    /// <summary>
    /// 替换Ground层特定位置上的实体（调用OnKilled）
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="name">替换者的名字</param>
    public GameObject KillGroundEntity(Vector2Int pos, GameObject go)
    {
        if (groundEntityDict.ContainsKey(pos))
        {
            KillEntity(groundEntityDict[pos]);
        }
        string name = ResServer.instance.GetEntityName(go.GetComponent<Entity>());
        curTilemap.ground[pos.x, pos.y] = name.ToLower();
        return groundLayer.AddEntity(name, pos);
    }

    /// <summary>
    /// 替换Unit层特定位置上的实体（不调用OnKilled）
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="name">替换者的内部名字</param>
    public GameObject ReplaceUnitEntity(Vector2Int pos, string name)
    {
        print($"ReplaceUnitEntity name {name} at {pos}");
        curTilemap.setting.Remove(pos);
        if (unitEntityDict.ContainsKey(pos))
        {
            GameObject go = unitEntityDict[pos].gameObject;
            unitEntityDict.Remove(pos);
            curTilemap.unit[pos.x, pos.y] = "";
            Destroy(go);
        }
        curTilemap.unit[pos.x, pos.y] = name.ToLower();
        return unitLayer.AddEntity(name, pos);
    }

    /// <summary>
    /// 为pos处的entity（仅unit层）添加setting
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddSetting(Vector2Int pos, string key, string value)
    {
        key = key.ToLower();
        if (!curTilemap.setting.ContainsKey(pos))
        {
            curTilemap.setting[pos] = new Dictionary<string, string>();
        }
        curTilemap.setting[pos][key] = value;
        if (unitEntityDict.ContainsKey(pos))
        {
            if (unitEntityDict[pos] is TriggerEntity)
            {
                var te = unitEntityDict[pos] as TriggerEntity;
                if (te.setting == null)
                {
                    te.setting = new Dictionary<string, string>();
                }
                te.setting[key] = value;
            }
        }
    }

    #endregion

    #region 触发区域（TriggerArea）

    public List<TriggerArea> triggerAreas { get; private set; } = new List<TriggerArea>();

    public void RefreshTriggerAreas()
    {
        triggerAreas = new List<TriggerArea>(curTilemap.triggerAreas);
    }

    public void KillTriggerArea(TriggerArea area)
    {
        triggerAreas.Remove(area);
        curTilemap.triggerAreas.Remove(area);
    }

    public void Save2Archive(Archive archive)
    {
        archive.mapArchive = new List<MapArchive>();
        foreach (var p in tilemapCache)
        {
            archive.mapArchive.Add(new MapArchive(p.Value));
        }
        archive.curMap = curTilemap.mapName;
    }

    /// <summary>
    /// 负责处理地图缓存和当前地图
    /// </summary>
    /// <param name="archive"></param>
    public void LoadArchive(Archive archive)
    {
        tilemapCache.Clear();
        mapnameList.Clear();
        // TODO
        foreach (var atm in archive.mapArchive)
        {
            var tm = atm.ToTilemap();
            
            AddMap(tm);
        }
        curTilemap = tilemapCache[archive.curMap];
        RefreshEntities(curTilemap);
    }
    #endregion

    
}
