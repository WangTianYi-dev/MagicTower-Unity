/*
 * file: CommonDef.cs
 * author: DeamonHook
 * feature: 通用的类型定义
 */

using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 单位类型
/// </summary>
public enum EntityType
{
    Ground,
    Enemy,
    Event,
    Item,
    Skill,
    Block
}

/// <summary>
/// 种族
/// </summary>
public enum Race
{
    Human,      //人族, 主角所在的种族
    Wildlife,   //野生动物, 如史莱姆, 蝙蝠等
    Skeleton,   //骷髅族, 魔族通过魔法操纵的生物
    Zombie,     //丧尸(兽人)族, 已死之人, 如格勒第, 被十字架克制
    Deamon,     //魔族, 魔界的主体民族
    Dragon      //龙族, 上个时代大陆的主宰, 被屠龙匕克制
}

public enum Height
{
    Ground = 0,
    Unit = 10,
    Line = 20,
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

    public Property propertyData;
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
    TilemapChange = 0x40,
    GetItem = 0x80,
    Block = 0x100, // 仅能在移动前事件中使用，阻挡玩家移动
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


