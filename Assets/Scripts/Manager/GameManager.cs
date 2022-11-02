/*
 * file: GameManager.cs
 * author: DeamonHook
 * feature: 游戏总管理器
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Assertions;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using System.IO;


public class GameManager : BaseManager
{
    public static GameManager instance { get; private set; }


    // xxmanager.instance的快捷方式
    private static MapManager mapmngr;
    private static UIManager uimngr;


    private void Awake()
    {
        instance = this;
        archivePath = System.IO.Path.Combine(Application.persistentDataPath, "Archives");
        Directory.CreateDirectory(archivePath);
    }

    public static Player player;

    // 快捷方式
    public Dictionary<string, Tilemap> name2Tilemap
    {
        get { return MapManager.instance.tilemapCache; }
    }


    private Dictionary<Vector2Int, Entity> groundEntityDict
    {
        get
        {
            return MapManager.instance.groundEntityDict;
        }
    }

    private Dictionary<Vector2Int, Entity> unitEntityDict
    {
        get
        {
            return MapManager.instance.unitEntityDict;
        }
    }


    private int mapWidth { get { return MapManager.instance.mapWidth; } }

    private int mapHeight { get { return MapManager.instance.mapHeight; } }

    public override void Init()
    {
        base.Init();
        instance = this;
        mapmngr = MapManager.instance;
        uimngr = UIManager.instance;
    }

    public void InitGame()
    {
        UIManager.instance.SwitchWindow("StartWindow");
    }

    public void NewGame()
    {
        UIManager.instance.RefreshUI();
        UIManager.instance.SwitchWindow("GameWindow");
        ShowPrompt(new List<string>
        {
            "童年小游戏《魔塔》的重制版，当然，有亿点不一样\n" +
            "游戏玩法：\n点击可以到达的区域，勇士将自动进行寻路，不要使用方向键，它一点也不好用，只是我为了视觉平衡加的" +
            "自动拾取，自动战斗，手残也能玩(脑残也许不能)\n" +
            "右边的按键都可以点，至于作用？请君一试便知。\n" +
            "本人保证可以过关，如果卡关？为什么不重新来一遍呢？\n" +
            "至于剧情，只是为了让游戏不那么寡淡而编出来的，不要在意",

            "前情提要：\n" +
            "距离不可一世的龙族被人、魔两族携手击败已有77年，时间逐渐治愈了战争的创伤，" +
            "可是，10年以前，魔族突然掳走了人族的公主，国王震怒，高价悬赏寻找公主。" +
            "传说公主被关在人魔边境的一座魔塔中，已有100名勇士相继进入魔塔探险，" +
            "其中，66名死亡，33名，1名失踪。\n" +
            "而我们的主角因为赌博欠下了高额债务，也打算去碰碰运气"
        });
    }

    #region 玩家移动
    /// <summary>
    /// 是否可抵达目标
    /// </summary>
    /// <param name="targetPos">目标点坐标</param>
    /// <returns></returns>
    private bool PlayerPassable(Vector2Int targetPos)
    {
        // 地图边界不可通行
        if (!Util.Inside(
            targetPos, Vector2Int.zero, new Vector2Int(mapWidth - 1, mapHeight - 1))
            )
        {
            return false;
        }

        return MapManager.instance.BlockPassable(targetPos);
    }


    /// <summary>
    /// 将玩家移动至方向前一格
    /// </summary>
    /// <param name="dir"></param>
    public bool MoveByDir(Vector2Int dir)
    {
        if (player.RecvMoveMsg(dir)) // 如果player接受了移动信息
        {
            var cord = player.logicPos + dir;
            BeforeMove(cord);
            if (PlayerPassable(cord)) // pass
            {
                player.StartMove(cord);
                AfterMove(cord);
            }
            else // blocked
            {
                AfterBlocked(cord);
            }
            UIManager.instance.RefreshUI();
            return true;
        }
        return false;
    }

    private Entity groundEntity, unitEntity; // 当前格子上的entity

    public void BeforeMove(Vector2Int cord)
    {
        groundEntityDict.TryGetValue(cord, out groundEntity);
        unitEntityDict.TryGetValue(cord, out unitEntity);
        if (groundEntity != null) groundEntity.BeforeCollision();
        if (unitEntity != null)
        {
            if (unitEntity is Enemy)
            {
                BuffBeforeBattle(unitEntity as Enemy);
            }
            unitEntity.BeforeCollision();
        }
    }

    public void AfterMove(Vector2Int cord)
    {
        if (groundEntity != null) groundEntity.AfterMoveTo();
        if (unitEntity != null)
        {
            unitEntity.AfterMoveTo();
        }
        if (unitEntity is Enemy)
        {
            BuffAfterBattle(unitEntity as Enemy);
            Util.KillEntity(unitEntity);
            RefreshPlayerExternalProperty();
        }
        PeekTriggerAreas(cord);
    }

    public void AfterBlocked(Vector2Int cord)
    {
        if (groundEntity != null) groundEntity.AfterBlocked();
        if (unitEntity != null) unitEntity.AfterBlocked();
    }


    /// <summary>
    /// 将玩家移动向坐标方向一格（变更动画）
    /// </summary>
    /// <param name="cord"></param>
    public bool MoveByPos(Vector2Int cord)
    {
        Vector2Int dir;
        if (cord.x > player.logicPos.x)
        {
            dir = Vector2Int.right;
        }
        else if (cord.x < player.logicPos.x)
        {
            dir = Vector2Int.left;
        }
        else if (cord.y > player.logicPos.y)
        {
            dir = Vector2Int.up;
        }
        else
        {
            dir = Vector2Int.down;
        }
        return MoveByDir(dir);
    }


    /// <summary>
    /// 玩家在游戏区域内点击
    /// </summary>
    /// <param name="x">x方向上的相对位置（0到1）</param>
    /// <param name="y">y方向上的相对位置（0到1）</param>
    public void TouchDown(float x, float y)
    {
        route.Clear();
        GroundLayer.instance.RenderLine(route);
    }

    /// <summary>
    /// 玩家在游戏区域内取消点击
    /// </summary>
    /// <param name="x">x方向上的相对位置（0到1）</param>
    /// <param name="y">y方向上的相对位置（0到1）</param>
    public void TouchUp(float x, float y)
    {
        route.Clear();
        if (Player.instance.playerState == State.Idle)
        {
            var passableGrid = MapManager.instance.GetPassableGrid();
            LinkedList<Vector2Int> rt = Router.Route(passableGrid,
                Player.instance.logicPos,
                new Vector2Int((int)(x * mapHeight), (int)(y * mapWidth)));

            if (rt.Count == 0)
            {
                UIManager.instance.PopMessage("目标不可到达");
            }
            else
            {

                GroundLayer.instance.RenderLine(rt);
                rt.RemoveFirst(); // 这一步是为了消除动画bug
            }
            route = rt;
        }
    }

    private LinkedList<Vector2Int> route = new LinkedList<Vector2Int>();

    /// <summary>
    /// 停止玩家移动并清空路径
    /// </summary>
    public void PlayerStop()
    {
        player.playerState = State.Idle;
        route.Clear();
        GroundLayer.instance.RenderLine(route);
    }

    private bool moveable = true;

    /// <summary>
    /// 暂停玩家移动（须在之前调用PlayerStop）
    /// </summary>
    public void PlayerSuspend()
    {
        moveable = false;
    }

    /// <summary>
    /// 暂停玩家移动并在duration之后恢复（须在之前调用PlayerStop）
    /// </summary>
    /// <param name="duration"></param>
    public void PlayerSuspend(float duration)
    {
        moveable = false;
        StartCoroutine(Util.InvokeAfterSeconds(duration, PlayerResume));
    }

    /// <summary>
    /// 恢复移动
    /// </summary>
    private void PlayerResume()
    {
        moveable = true;
    }

    private void CheckInput()
    {
        if (route.Count > 0) // 若存在路径
        {
            if (MoveByPos(route.First.Value)) // 若勇士接收到信息
            {
                if (route.Count > 0) 
                    route.RemoveFirst();
                GroundLayer.instance.RenderLine(route);
            }
        }
        else if (DirButton.instance.curDir != Vector2Int.zero)
        {
            MoveByDir(DirButton.instance.curDir);
        }
    }
    #endregion

    #region 战斗管理

    /// <summary>
    /// 战斗之前勇者上buff
    /// </summary>
    /// <param name="e"></param>
    public void BuffBeforeBattle(Enemy e)
    {
        Property p = player.property, ep = e.property;
        
        var unaryBuffs = from buff in playerBuff
                         where buff.unaryAddition != null && buff.launchTime == Buff.LaunchTime.BeforeBattle
                         select buff;
        foreach (var ub in unaryBuffs)
        {
            ub.unaryAddition(ref p);
        }

        var binaryBuffs = from buff in playerBuff
                    where buff.binaryAddition != null && buff.launchTime == Buff.LaunchTime.BeforeBattle
                    select buff;
        foreach (var bb in binaryBuffs)
        {
            bb.binaryAddition(ref p, ref ep);
        }
        RefreshPlayerExternalProperty();
        e.externalProperty = ep;
    }

    /// <summary>
    /// 战斗之后勇者上/卸buff
    /// </summary>
    /// <param name="e"></param>
    public void BuffAfterBattle(Enemy e)
    {
        print($"unbuff after {e.nameInGame}");
        // 与战前不同，直接改变player.property
        var unaryBuffs = from buff in playerBuff
                         where buff.unaryAddition != null && buff.launchTime == Buff.LaunchTime.AfterBattle
                         select buff;
        foreach (var ub in unaryBuffs)
        {
            ub.unaryAddition(ref player.property);
        }

        var binaryBuffs = from buff in playerBuff
                          where buff.binaryAddition != null && buff.launchTime == Buff.LaunchTime.AfterBattle
                          select buff;
        foreach (var bb in binaryBuffs)
        {
            bb.binaryAddition(ref player.property, ref e.property);
        }

        // 瞬时加成战后消失
        var instantBuffs = from buff in playerBuff
                           where buff.type == Buff.AdditionType.Instant
                           select buff;
        List<Buff> cache = new List<Buff>(instantBuffs);
        foreach (var ib in cache)
        {
            CancelPlayerBuff(ib);
        }
        player.curSkill = "";
        RefreshPlayerExternalProperty();
        uimngr.RefreshUI();
    }


    #region 装备系统

    private Equipment GetEquipment(string name)
    {
        return ResServer.instance.GetObject(name).GetComponent<Equipment>();
    }

    private void AddEquipmentBuff(string name)
    {
        foreach (var b in Equipment.EquipmentBuffDict[name])
        {
            AddPlayerBuff(b);
        }
    }

    private void CancelEquipmentBuff(string name)
    {
        foreach (var b in Equipment.EquipmentBuffDict[name])
        {
            CancelPlayerBuff(b);
        }
    }


    public void PlayerWearEquipment(string name)
    {
        AddEquipmentBuff(name);
        Equipment toDress = GetEquipment(name);
        var toUndress = from e in player.equipmentsWeared
                        where GetEquipment(e).equipmentType == toDress.equipmentType
                        select e;
        var toRmv = new HashSet<string>();
        foreach (var n in toUndress)
        {
            CancelEquipmentBuff(n);
            toRmv.Add(n);
        }
        foreach (var n in toRmv)
        {
            player.equipmentsWeared.Remove(n);
        }
        player.equipmentsWeared.Add(name);
        uimngr.RefreshUI();
    }


    #endregion

    #region 加成效果

    /*
     * 将一元/二元加成效果分为5类
     * 1. 永久加成，如宝石、血瓶
     * 2. 持续加成，战斗前生效，如剑/盾
     * 3. 持续加成，战斗后生效，如幸运金币
     * 4. 瞬时加成，战斗前生效，如凡骨、皇者
     * 5. 瞬时加成，战斗后生效，如流石
     * 其中，(一元&&战前)加成效果会显示在面板上，即改变externalProperty
     */

    // TODO 可能会优化
    // 目前生效的加成效果
    private List<Buff> playerBuff = new List<Buff>();


    public void RefreshPlayerExternalProperty()
    {
        Property p = player.property;
        var unaryBuffs = from buff in playerBuff
                         where buff.unaryAddition != null && buff.launchTime == Buff.LaunchTime.AllTime
                         select buff;
        foreach (var buff in unaryBuffs)
        {
            buff.unaryAddition(ref p);
        }
        player.externalProperty = p;
    }


    public void AddPlayerBuff(Buff a)
    {
        // 永久加成立即生效
        if (a.type == Buff.AdditionType.Persistant)
        {
            a.unaryAddition(ref player.property);
        }
        else
        {
            playerBuff.Add(a);
        }
        RefreshPlayerExternalProperty();
    }


    public void CancelPlayerBuff(Buff a)
    {
        playerBuff.Remove(a);
    }


    #region （废弃的加成）将会重写
    /// <summary>
    /// 简单的string->Addition正则
    /// </summary>
    private static Regex additionRegex = new Regex(@"^\s*(?<key>[^:\s]+)\s*:\s*(?<value>[+-]?\d+)\s*");


    /// <summary>
    /// 应用加成效果
    /// </summary>
    /// <param name="addition">一个加成效果</param>
    public void ApplyAddition(string addition)
    {
        var match = additionRegex.Match(addition);
        if (!match.Success)
        {
            Debug.LogError($"{addition} 不是一个合法的加成");
            return;
        }
        string key = match.Groups["key"].Value.ToLower();
        long count = long.Parse(match.Groups["value"].Value);
        if (additionHandlerDict.ContainsKey(key))
        {
            additionHandlerDict[key](count);
        }
        else
        {
            Debug.LogError($"未知的加成项: {key}");
        }
        RefreshPlayerExternalProperty();
    }

    public delegate void AdditionHandler(long count);

    private static void AttackAdd(long count)
    {
        Player.instance.property.ATK += count;
    }

    private static void DefenseAdd(long count)
    {
        Player.instance.property.DEF += count;
    }

    private static void HitpointAdd(long count)
    {
        Player.instance.property.HP += count;
    }

    private static void CoinAdd(long count)
    {
        Player.instance.property.Coin += count;
    }

    private static void YellowKeyAdd(long count)
    {
        Player.instance.property.yellowKey += count;
    }

    private static void BlueKeyAdd(long count)
    {
        Player.instance.property.blueKey += count;
    }

    private static void RedKeyAdd(long count)
    {
        Player.instance.property.redKey += count;
    }

    public Dictionary<string, AdditionHandler> additionHandlerDict =
        new Dictionary<string, AdditionHandler>
        {
            {"attack", AttackAdd },
            {"defense", DefenseAdd },
            {"hitpoint", HitpointAdd },
            {"coin", CoinAdd },
            {"yellowkey", YellowKeyAdd },
            {"bluekey", BlueKeyAdd },
            {"redkey", RedKeyAdd },
        };
    #endregion
    #endregion
    #endregion
    #region 障碍（门）处理

    public delegate bool RequirementAsker(long amount);

    private static bool YellowKeyAsker(long amount)
    {
        if (player.property.yellowKey >= amount)
        {
            player.property.yellowKey -= amount;
            return true;
        }
        return false;
    }

    private static bool BlueKeyAsker(long amount)
    {
        if (player.property.blueKey >= amount)
        {
            player.property.blueKey -= amount;
            return true;
        }
        return false;
    }

    private static bool RedKeyAsker(long amount)
    {
        if (player.property.redKey >= amount)
        {
            player.property.redKey -= amount;
            return true;
        }
        return false;
    }

    private Dictionary<string, RequirementAsker> askerDict = new Dictionary<string, RequirementAsker>
    {
        {"yellowkey", YellowKeyAsker },
        {"bluekey", BlueKeyAsker },
        {"redkey", RedKeyAsker }
    };

    /// <summary>
    /// 所有需求必须遵守的正则
    /// </summary>
    private static Regex requirementRegex = new Regex(@"^\s*(?<key>[^:\s]+)\s*:\s*(?<value>[+-]?\d+)\s*");

    /// <summary>
    /// 询问是否满足条件，若满足，则进行相应的钥匙扣除等操作
    /// </summary>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public bool AskRequirement(string requirement)
    {
        if (requirement == "") return false;
        var match = requirementRegex.Match(requirement);
        if (!match.Success)
        {
            Debug.LogError($"{requirement} 不是一个合法的需求");
            return false;
        }
        string key = match.Groups["key"].Value.ToLower();
        long count = long.Parse(match.Groups["value"].Value);
        if (askerDict.ContainsKey(key))
        {
            return askerDict[key](count);
        }
        else
        {
            Debug.LogError($"未知的需求项: {key}");
            return false;
        }
    }
    #endregion

    #region 菜单项相关

    public List<Enemy> GetEnemys()
    {
        var enemySet = new HashSet<Enemy>();
        var nameSet = new HashSet<string>();
        foreach (var enemy in MapManager.instance.unitEntityDict.Values)
        {
            if (enemy is Enemy)
            {
                if (!nameSet.Contains(enemy.nameInGame))
                {
                    nameSet.Add(enemy.nameInGame);
                    enemySet.Add(enemy as Enemy);
                }
            }
        }

        return enemySet.ToList();
    }

    #endregion

    #region 地图设置处理

    /// <summary>
    /// 根据坐标获取地图中设置的拷贝
    /// </summary>
    /// <param name="cord">坐标</param>
    /// <returns></returns>
    public Dictionary<string, string> RequestSetting(Vector2Int cord)
    {
        return new Dictionary<string, string>(MapManager.instance.GetSettingAtPos(cord));
    }


    #endregion

    #region 事件处理

    private class EventNode
    {
        public bool done;
        public bool invoked;
        public Action action;
        public EventNode(Action action)
        {
            this.action = action;
            done = false;
            invoked = false;
        }
        public void Invoke()
        {
            action();
            invoked = true;
        }
    }

    Queue<EventNode> eventQueue = new Queue<EventNode>();

    private void HandleEvent()
    {
        if (eventQueue.Count > 0)
        {
            if (eventQueue.First().done)
            {
                eventQueue.Dequeue();
            }
            else if (!eventQueue.First().invoked)
            {
                eventQueue.First().Invoke();
            }
        }
    }

    public void CurrentEventDone()
    {
        if (eventQueue.Count > 0)
            eventQueue.First().done = true;
    }

    public void AddEvent(Action action)
    {
        eventQueue.Enqueue(new EventNode(action));
    }

    private void HandleTrigger(Dictionary<string, string> settings, Entity entity = null)
    {
        if (settings.ContainsKey("script"))
        {
            AddEvent(() => { ScriptManager.instance.DoString(settings["script"]); CurrentEventDone(); });
        }
        switch (settings["type"])
        {
            case "chat":
                {
                    AddEvent(() =>
                        {
                            var dialog = Parser.LoadDialog(settings["dialog"]);
                            ShowDialog(dialog);
                        }
                    );
                }
                break;
            case "trade":
                {
                    AddEvent(() =>
                        {
                            string raw = settings["transactions"];
                            List<Func<bool>> transactions = ParseTransactions(raw);
                            string message = settings["message"];
                            uimngr.OpenWindow("StoreWindow");
                            StoreWindow.instance.Refresh(message, transactions, settings.ContainsKey("dieafterdone"));
                        }   
                    );
                }
                break;

            case "altar":
                {
                    AddEvent(() =>
                    {
                        if (entity == null)
                        {
                            Debug.LogError($"triggerType altar must bind to an entity!");
                            return;
                        }
                        var altar = entity as Altar;
                        uimngr.OpenWindow("StoreWindow");
                        StoreWindow.instance.Refresh(altar.message, altar.transactions);
                    });
                }
                break;

            case "transport":
                {
                    AddEvent(() =>
                    {
                        if (settings["target"].ToLower().Contains("lastpos"))
                        {
                            Regex r = new Regex(@"\s*\[\s*(?<mapName>[^\s,]+)");
                            Match ma = r.Match(settings["target"]);
                            if (ma.Success)
                            {
                                settings["target"] = settings["target"].ToLower();
                                var l = mapmngr.tilemapCache[ma.Groups["mapName"].Value].lastPos;
                                settings["target"] = settings["target"].Replace("lastpos", $"{l.x}, {l.y}");
                            }
                        }
                        var m = targetRgx.Match(settings["target"]);
                        if (m.Success)
                        {
                            print($"mapName: {m.Groups["mapName"]}, {m.Groups["x"]}, {m.Groups["y"]}");
                            StartCoroutine(
                                // 使用协程以避免组件执行顺序问题可能产生的bug
                                PlayerTransPortCoroutine(
                                    m.Groups["mapName"].Value,
                                    new Vector2Int(
                                        int.Parse(m.Groups["x"].Value),
                                        int.Parse(m.Groups["y"].Value)
                                    )
                                )
                            );
                        }
                        else
                        {
                            Debug.LogError($"非法的目标：{settings["target"]}");
                        }
                        CurrentEventDone();
                    });
                }
                break;
            case "none" or "":
                break;
            default:
                Debug.LogError($"不支持的触发类型：{settings["type"]}");
                break;
        }

    }


    /// <summary>
    /// 触发结束的事件
    /// </summary>
    ///// 
    //[HideInInspector]
    //public UnityEvent triggerOver = new UnityEvent();

    //public void TriggerSuccess()
    //{
    //    triggerOver.Invoke();
    //    triggerOver.RemoveAllListeners();
    //}

    //public void TriggerFailed()
    //{
    //    triggerOver.RemoveAllListeners();
    //}

    /// <summary>
    /// 传送目标的正则
    /// </summary>
    Regex targetRgx = new Regex(@"\[\s*(?<mapName>[^\s,]+)\s*,\s*(?<x>\d+)\s*,\s*(?<y>\d+)\s*\]");

    /// <summary>
    /// 由实体激活的触发
    /// </summary>
    /// <param name="entity">触发实体</param>
    public void TriggerByEntity(TriggerEntity entity)
    {
        Dictionary<string, string> settings = entity.setting;
        HandleTrigger(settings, entity);
        AddEvent(() => { entity.OnTriggerDone(); CurrentEventDone(); });
    }


    /// <summary>
    /// mapmanager的同名方法的快捷方式
    /// </summary>
    /// <param name="e"></param>
    public void RemoveEntity(Entity e)
    {
        mapmngr.KillEntity(e);
    }

    public void Transport(string mapName, Vector2Int pos)
    {
        StartCoroutine(PlayerTransPortCoroutine(mapName, pos));
    }

    /// <summary>
    /// 玩家传送事件处理
    /// </summary>
    /// <param name="map">目标地图</param>
    /// <param name="pos">目标位置</param>
    private IEnumerator PlayerTransPortCoroutine(string map, Vector2Int pos)
    {
        yield return null;
        print($"Transport target: {map}, {pos}");
        PlayerStop();
        float suspendTime = 0.5f;
        PlayerSuspend(suspendTime);
        if (mapmngr.curTilemap.mapName != map)
        {
            mapmngr.ChangeMap(map);
        }
        player.TransportTransform(pos);
        StartCoroutine(PlayerTransPortCoroutine(pos));
    }


    private IEnumerator PlayerTransPortCoroutine(Vector2Int pos)
    {
        yield return null;
        PlayerStop();
        player.TransportTransform(pos);
        AfterMove(pos);
    }


    // 显示对话
    public void ShowDialog(List<string> dialog)
    {
        UIManager.instance.OpenWindow("DialogWindow");
        DialogWindow.instance.RefreshDialog(dialog);
    }

    // 显示提示信息
    public void ShowPrompt(List<string> prompts)
    {
        uimngr.OpenWindow("PromptWindow");
        PromptWindow.instance.RefreshPrompt(prompts);
    }

    #endregion


    #region 物品处理

    public void GetItem(string name)
    {
        player.GetItem(name);
    }

    public void UseItem(string name)
    {
        if (Item.ItemEffects.ContainsKey(name))
        {
            if (Item.ItemEffects[name]())
            {
                player.ConsumeItem(name);
            }
        }
        else
        {
            print($"{name}'s effect not exist");
        }
    }

    #endregion

    #region 技能处理
    //public void PlayerWearEquipment(string name)
    //{
    //    AddEquipmentBuff(name);
    //    Equipment toDress = GetEquipment(name);
    //    var toUndress = from e in player.equipmentsWeared
    //                    where GetEquipment(e).equipmentType == toDress.equipmentType
    //                    select e;
    //    var toRmv = new HashSet<string>();
    //    foreach (var n in toUndress)
    //    {
    //        CancelEquipmentBuff(n);
    //        toRmv.Add(n);
    //    }
    //    foreach (var n in toRmv)
    //    {
    //        player.equipmentsWeared.Remove(n);
    //    }
    //    player.equipmentsWeared.Add(name);
    //    uimngr.RefreshUI();
    //}

    public void UseSkill(string name)
    {
        if (Skill.SkillBuffDict.ContainsKey(name) && Skill.prerequsiteDict.ContainsKey(name))
        {
            if (Skill.prerequsiteDict[name]() == false)
            {
                uimngr.PopMessage("技能使用失败");
                return;
            }
            foreach (var buff in Skill.SkillBuffDict[name])
            {
                AddPlayerBuff(buff);
            }
            player.curSkill = name;
            uimngr.RefreshUI();
        }
        else
        {
            print($"{name}'s effect not exist");
        }
    }


    #endregion
    #region 交易


    /// <summary>
    /// 祭坛交易次数
    /// </summary>
    /// 
    [HideInInspector]
    public int altarCount = 0;


    // 可以改进以支持更多交易形式
    public static Regex transactionRegex = new
        Regex(@"(?<prop1>\S+)\s*:(?<amount1>\d+)\s*\|\s*(?<prop2>\S+)\s*:(?<amount2>\d+)");


    /// <summary>
    /// 将transaction设定转换为回调
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    List<Func<bool>> ParseTransactions(string raw)
    {
        var transactions = new List<Func<bool>>();
        transactions.Clear();
        MatchCollection matches = transactionRegex.Matches(raw);
        foreach (Match m in matches)
        {
            transactions.Add(CreateTransaction(m.Groups["prop1"].Value, m.Groups["amount1"].Value,
                m.Groups["prop2"].Value, m.Groups["amount2"].Value)
            );
        }
        return transactions;
    }


    /// <summary>
    /// 创建交易的lambda
    /// </summary>
    public Func<bool> CreateTransaction(
        string costProp, string CostAmount, string purchaseProp, string purchaseAmount
        )
    {
        long r1 = long.Parse(CostAmount), r2 = long.Parse(purchaseAmount);
        return
            () =>
            {
                long a1 = Player.instance.GetPropertyValue(costProp);
                long a2 = Player.instance.GetPropertyValue(purchaseProp);
                if (a1 < r1)
                {
                    return false;
                }
                a1 -= r1;
                a2 += r2;
                Player.instance.SetPropertyValue(costProp, a1);
                Player.instance.SetPropertyValue(purchaseProp, a2);
                return true;
            };
        
    }



    #endregion

    #region 触发区域

    private void PeekTriggerAreas(Vector2Int cord)
    {
        foreach (var tArea in mapmngr.triggerAreas)
        {
            TriggerAreaHandler(tArea, cord);
        }
    }

    /// <summary>
    /// 前提条件判断器
    /// </summary>
    Dictionary<string, Func<TriggerArea, Vector2Int, bool>> prerequsiteJudges = new Dictionary<string, Func<TriggerArea, Vector2Int, bool>>
    {
        {
            "collision",
            (area, cord) =>
            {
                return area.position == cord;
            }
        }
    };

    private void TriggerAreaHandler(TriggerArea area, Vector2Int cord)
    {
        string prerequsite = area.settings.ContainsKey("prerequsite") ? area.settings["prerequsite"] : "collision";
        if (prerequsiteJudges[prerequsite](area, cord))
        {
            //PlayerStop();
            TriggerByArea(area);
        }
    }


    private UnityAction OnTriggerAreaDone(TriggerArea area)
    {
        UnityAction action = new UnityAction(()=> { });
        //foreach (var kvp in area.settings)
        //{
        //    print($"{kvp.Key}, {kvp.Value}");
        //}
        if (area.settings.ContainsKey("dieafterdone"))
        {
            action += () =>
            {
                mapmngr.KillTriggerArea(area);
            };
        }
        return action;
    }

    /// <summary>
    /// 由区域激活的触发
    /// </summary>
    private void TriggerByArea(TriggerArea area)
    {
        var settings = area.settings;
        HandleTrigger(settings);
        AddEvent(() => { OnTriggerAreaDone(area)(); CurrentEventDone(); PlayerResume(); });
    }

    #endregion

    #region 楼层传送

    /// <summary>
    /// 获取楼层传送窗口所需要的信息
    /// </summary>
    /// <returns></returns>
    public List<(string, Action)> GetFlyList()
    {
        var list = new List<(string, Action)>();
        foreach (string str in mapmngr.mapnameList)
        {

            Tilemap tilemap = mapmngr.tilemapCache[str];
            Vector2Int targetPos = tilemap.lastPos;
            var mapName = str;
            Action action = () =>
            {
                print($"flybutton: {mapName}, {targetPos}");
                UIManager.instance.SwitchWindow("GameWindow");
                Transport(mapName, targetPos);
            };
            list.Add((mapName, action));
        }
        return list.ToList();
    }

    #endregion


    public Dictionary<string, string> globalDict = new Dictionary<string, string>();

    #region 游戏存档/读档

    private string archivePath;


    private byte[] Encryption(string raw)
    {
        return System.Text.Encoding.Default.GetBytes(raw);
    }

    private string Decryption(byte[] bytes)
    {
        return System.Text.Encoding.Default.GetString(bytes);
    }

    private void SaveFileWithEncryption(FileStream fs, string content)
    {
        byte[] bytes = Encryption(content);
        fs.Write(bytes);
    }

    private string LoadWithDecryption(FileStream fs)
    {
        List<byte> bytes = new List<byte>();
        Int32 b;
        while ((b = fs.ReadByte()) != -1)
        {
            bytes.Add((byte)b);
        }
        return Decryption(bytes.ToArray());
    }


    private void SaveArchiveFile(string path)
    {
        var archive = CreateArchive();
        var serial = "DH" + JsonUtility.ToJson(archive);
        var bytes = Encryption(serial);
        var fs = new FileStream(path, FileMode.Create);
        fs.Write(bytes);
        fs.Close();
    }


    private Archive CreateArchive()
    {
        var archive = new Archive();
        archive.version = 0;
        archive.mapArchive = new List<MapArchive>();
        mapmngr.Save2Archive(archive);
        archive.curPos = player.logicPos;
        archive.altarCount = altarCount;
        (archive.globalValues, archive.globalKeys) = Util.CreateKeysAndValuesList(globalDict);
        player.SaveArchive(archive);
        return archive;
    }


    /// <summary>
    /// 存档
    /// </summary>
    /// <param name="index">序号</param>
    public void SaveGame(int index)
    {
        var p = Path.Combine(archivePath, $"{index}.dhd");
        SaveArchiveFile(p);
        MetaArchive marc = new MetaArchive
        {
            version = 0,
            index = index,
            areaName = mapmngr.curTilemap.mapName,

            HP = player.property.HP.ToString(),
            ATK = player.property.ATK.ToString(),
            DEF = player.property.DEF.ToString(),
            Coin = player.property.Coin.ToString(),
        };
        var json = JsonUtility.ToJson(marc);
        var f = new FileStream(Path.Combine(archivePath, $"{index}.dhm"), FileMode.Create);
        var sw = new StreamWriter(f);
        sw.Write(json);
        sw.Close();
        f.Close();
    }

    // 被ArchiveWindow调用
    /// <summary>
    /// 获得按照index排序的meta存档（meta存档是一种小存档，不包含具体游戏数据）
    /// </summary>
    /// <returns></returns>
    public List<MetaArchive> GetMetasOrdered()
    {
        print(archivePath);
        var archiveRgx = new Regex(@"\d+\.dhd");
        var metaRgx = new Regex(@"\d+\.dhm");
        var l = new List<int>();
        var finfos = new HashSet<FileInfo>(new DirectoryInfo(archivePath).GetFiles());
        var lma = new List<MetaArchive>();
        foreach (var info in finfos)
        {
            if (metaRgx.IsMatch(info.Name))
            {
                string dname = info.Name.Substring(0, info.Name.Length-1);
                dname += 'm';
                if (finfos.Any(f => f.Name == dname))
                {
                    var fs = info.OpenRead();
                    var str = LoadWithDecryption(fs);
                    lma.Add(JsonUtility.FromJson<MetaArchive>(str));
                    fs.Close();
                }
            }
        }
        var names = from ma in lma
                    orderby ma.index
                    select ma;
        return new List<MetaArchive>(names);
    }


    private void LoadArchive(Archive archive)
    {
        eventQueue.Clear();
        player.LoadArchive(archive);
        mapmngr.LoadArchive(archive);
        uimngr.RefreshUI();
        altarCount = archive.altarCount;
        globalDict = Util.CreateDictionaryViaLists(archive.globalKeys, archive.globalValues);
        playerBuff.Clear();
        StartCoroutine(PlayerTransPortCoroutine(archive.curPos));
    }


    /// <summary>
    /// 假定“{index}.dhd”存在，读取游戏
    /// </summary>
    /// <param name="index">索引</param>
    public void LoadGame(int index)
    {
        string dname = $"{index}.dhd";
        FileStream fs = new FileStream(Path.Combine(archivePath, dname), FileMode.Open, FileAccess.Read);
        int i;
        var bytes = new List<byte>();
        while ((i = fs.ReadByte()) != -1)
        {
            bytes.Add((byte)i);
        }
        fs.Close();
        byte[] bytea = bytes.ToArray();
        string str = Decryption(bytea);
        Assert.IsTrue(str.StartsWith("DH")); // 防伪标记
        str = str[2..];
        //print(str);
        Archive archive = JsonUtility.FromJson<Archive>(str);
        LoadArchive(archive);
        RefreshPlayerExternalProperty();
        UIManager.instance.RefreshUI();
    }


    public void DeleteAllArchives()
    {
        FileInfo[] flst = new DirectoryInfo(archivePath).GetFiles();
        foreach (var f in flst)
        {
            f.Delete();
        }
    }


    #endregion

    public void ParameterNotFound(EventType type)
    {
        Application.Quit();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (UIManager.instance.currentWindow == "GameWindow" && moveable)
            CheckInput();
        if (eventQueue.Count > 0)
        {
            HandleEvent();
        }
    }

    
}
