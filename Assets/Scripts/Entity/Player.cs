/*
 * file: Player.cs
 * author: DeamonHook
 * feature: 控制玩家(勇者)行为的类
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;

public class Player : Figure
{
    
    // 魔塔游戏只有一个玩家，所以使用单例模式以简化代码
    public static Player instance { get; private set; }

    #region 移动和动画
    private Vector2Int _playerDir;

    private Animator animator;

    private static Dictionary<Vector2Int, string> dirDict = new Dictionary<Vector2Int, string>
    {
        { Vector2Int.down, "Down" },
        { Vector2Int.up, "Up" },
        { Vector2Int.left, "Left" },
        { Vector2Int.right, "Right" }
    };

    
    /// <summary>
    ///  方向变换
    /// </summary>
    public Vector2Int playerDir
    {
        get { return _playerDir; }
        set
        {
            // 当且仅当运动方向改变时才触发动画的变化
            if (_playerDir == value) return;
            animator.SetTrigger(dirDict[value]);
            _playerDir = value;
        }
    }

    /// <summary>
    /// 获取玩家面对方向的下一个坐标
    /// </summary>
    private Vector2Int targetPos;


    public State playerState
    {
        get { return base.state; }
        set
        {
            if (base.state == value) return;

            // 当且仅当进入或离开移动状态时才设置isMove
            if (value == State.Move)
            {
                animator.SetFloat("moving", 1f);
            }
            else
            {
                animator.SetFloat("moving", 0f);
            }

            base.state = value;
        }
    }

    public float moveSpeed = 5.0f;

    /// <summary>
    /// 接受玩家移动消息
    /// </summary>
    /// <param name="dir">移动方向</param>
    /// <returns>是否接收到消息</returns>
    public bool RecvMoveMsg(Vector2Int dir)
    {
        if (base.state == State.Idle)
        {
            playerDir = dir;
            return true;
        }
        return false;
    }


    /// <summary>
    /// 确认可以移动后开始移动
    /// </summary>
    public void StartMove(Vector2Int cord)
    {
        targetPos = cord;
        playerState = State.Move;
    }


    /// <summary>
    /// Transform组件的移动(不负责任何其他逻辑)
    /// </summary>
    /// <returns>当组件移动到目标后返回true, 若尚未移动至目标则返回false</returns>
    private bool MoveTransform()
    {
        Vector3 tpos = Util.ApplyOffset(this.transform.gameObject, targetPos);
        transform.position = Vector3.MoveTowards(
            transform.position, tpos, moveSpeed * Time.deltaTime
            );
        if (transform.position == tpos) { logicPos = targetPos; return true; }
        else return false;
    }

    /// <summary>
    /// Transform组件的瞬移(同时移动逻辑位置)
    /// </summary>
    public void TransportTransform(Vector2Int targetPos)
    {
        logicPos = targetPos;
        Vector3 tpos = Util.ApplyOffset(this.transform.gameObject, targetPos);
        transform.position = tpos;
    }

    private List<Action> actionAfterMoved = new List<Action>(); // 勇士移动完成之后触发


    /// <summary>
    /// 注册当前移动完成后的动作
    /// </summary>
    /// <param name="action"></param>
    public void RegisterAfterMovedAction(Action action)
    {
        actionAfterMoved.Add(action);
    }

    private void Update()
    {
        switch (playerState)
        {
            case State.Move:
                if (MoveTransform()) // 如果当前移动完成
                {
                    playerState = State.Idle;
                    foreach (var action in actionAfterMoved)
                    {
                        action.Invoke();
                    }
                    actionAfterMoved.Clear();
                }
                break;
            case State.Idle:
                break;
            default:
                break;
        }
    }

    #endregion

    #region 技能和物品



    protected override void Awake()
    {
        base.Awake();
        instance = this;
        animator = GetComponent<Animator>();
    }



    // 物品列表
    public Dictionary<string, int> items { get; private set; } = new Dictionary<string, int>();
    public void GetItem(string name)
    {
        if (items.ContainsKey(name))
            items[name]++;
        else
        {
            items[name] = 1;
        }
    }

    // 仅负责减少计数，不负责其他逻辑
    public void ConsumeItem(string name)
    {
        items[name]--;
    }

    // 装备集合
    public HashSet<string> equipmentsWeared { get; private set; } = new HashSet<string>();
    public HashSet<string> equipments { get; private set; } = new HashSet<string>();
    public HashSet<string> skills { get; private set; } = new HashSet<string>();
    public string curSkill = "";

    public long GetPropertyValue(string name)
    {
        name = name.ToLower();
        long l = -1;
        switch (name)
        {
            case "hp":
            case "hitpoint":
                return property.HP;
            case "atk":
            case "attack":
                return property.ATK;
            case "def":
            case "defense":
                return property.DEF;
            case "coin":
                return property.Coin;
        }
        return l;
    }


    public long SetPropertyValue(string name, long value)
    {
        name = name.ToLower();
        long l = -1;
        switch (name)
        {
            case "hp":
            case "hitpoint":
                property.HP = value;
                break;
            case "atk":
            case "attack":
                property.ATK = value;
                break;
            case "def":
            case "defense":
                property.DEF = value;
                break;
            case "coin":
                property.Coin = value;
                break;
        }
        return l;
    }

    #endregion

    #region 存/读档

    /// <summary>
    /// 负责玩家数据的读取
    /// </summary>
    /// <param name="archive"></param>
    public void LoadArchive(Archive archive)
    {
        this.property = archive.playerProperty;

        this.items = new Dictionary<string, int>();
        foreach (var p in archive.playerItems)
        {
            this.items.Add(p.name, p.count);
        }
        this.skills = new HashSet<string>(archive.playerSkills);
        this.equipments.Clear();
        foreach (var s in archive.playerEquipments)
        {
            this.equipments.Add(s);
        }
        this.equipmentsWeared.Clear();
    }


    public void SaveArchive(Archive archive)
    {
        archive.playerProperty = this.property;
        archive.playerSkills = new List<string>();
        foreach (var s in this.skills)
        {
            archive.playerSkills.Add(s);
        }
        archive.playerItems = new List<ItemPair>();
        foreach (var i in this.items)
        {
            archive.playerItems.Add(new ItemPair{name=i.Key, count=i.Value});
        }
        archive.playerEquipments = new List<string>();
        foreach (var s in this.equipments)
        {
            archive.playerEquipments.Add(s);
        }
    }

    protected override void Start()
    {
        base.Start();
        
    }

    #endregion


}

