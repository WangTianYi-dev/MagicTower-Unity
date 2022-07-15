/*
 * file: Common.cs
 * author: DeamonHook
 * date: 7/6/2022
 * feature: Common enums and definations
 */

using UnityEngine;
using System.Collections.Generic;
using System;

public enum UnitType
{
    Enemy,
    Door,
    Item,
    Stair,
    NPC,
    Player,
    EventPoint,
    Shop,
}

public class GroundData
{
    public int floor;
    public string data;
}

public class MulchData
{
    public int floor;
    public string enemyData;
    public string doorData;
    public string itemData;
    public string stairsData;
    public string npcData;
    public string evtPointData;
    public string shopData;
}

public class PlayerData
{
    //位置
    public int floor;
    public Vector2Int pos;

    public PropertyData propertyData;
}

public class PropertyData
{
    //钥匙
    public int yellowKey;
    public int blueKey;
    public int redKey;
    //属性
    public int hp;
    public int atk;
    public int def;
    public int coin;
    //道具
    public int[] items;        //对应15种道具
    //最高层数
    public int maxFloor;
    //是否有圣盾
    public bool isImmune;
}

public class EventData          //一个mulch所含子事件的元素
{
    public EventType eventType;

    //增加/消耗钥匙
    public int keyType;         //0,1,2 对应 黄,蓝,红
    public int keyCount;

    //上下楼
    public int floor;
    public Vector2Int pos;
    public Vector2Int tPos;

    //对话
    public string word;

    //属性
    public PropertyType propertyType;
    public int deltaProprety;

    //获得道具
    public int itemType;

    //特效
    public string effectName;

    //创建覆盖物
    public string mulchName;
    public int eventIdToCreat;
    public bool isCover;
    public bool isChangeMulchOnEvt;

    //改变地图
    public string tileName;

    //等待
    public float waitTime;

    //事件回溯的索引
    public int index;

    //TODO 增加新的事件
}

/// <summary>
/// 事件类型
/// </summary>
public enum EventType
{
    None = 0x0,
    PropertyChange = 0x1,
    Destory = 0x2,
    Combat = 0x4,
    ToIdle = 0x8,
    PlayerMove = 0x10,
    Dialogue = 0x20,
    CreateUnit = 0x40,
}

/// <summary>
/// 全局变量(不要滥用)
/// </summary>
public class GLOBAL
{
    public static readonly Vector2Int LEFT = new Vector2Int(-1, 0), RIGHT = new Vector2Int(1, 0);
    public static readonly Vector2Int UP = new Vector2Int(0, 1), DOWN = new Vector2Int(0, -1);
}

public class EventCallBack
{
    public CallBackType callBackType;
}

public enum CallBackType
{
    None,
    BattleVictory,              //战斗胜利
    BattleFailure,              //打不过
    EndTalk,                    //结束对话
}

public enum Direction
{
    Up, Down, Left, Right
}

/// <summary>
/// 单位当前的状态
/// </summary>
public enum State
{
    Idle, Move, OnCombat, OnEvent
}


/// <summary>
/// 战斗数据
/// </summary>
public struct CombatCount
{
    public int playerHp;
    public int enemyHp;
    public bool isPlayerAttack;

    public CombatCount(int playerHp, int enemyHp, bool isPlayerAttack)
    {
        this.playerHp = playerHp;
        this.enemyHp = enemyHp;
        this.isPlayerAttack = isPlayerAttack;
    }
}

public enum PropertyType
{
    Hp = 0,
    Atk = 1,
    Def = 2,
    Coin = 3,
}


