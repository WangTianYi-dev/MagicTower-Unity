/*
 * file: GameRoot.cs
 * author: DeamonHook
 * feature: 游戏的入口
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [Header("初始地图")]
    public string originMap;
    [Header("生成主角的位置")]
    public Vector2Int playerPos;

    public GameObject playerPrefab;
    /// <summary>
    /// 地面层和单位层
    /// </summary>
    private GameObject groundLayer, unitLayer;

    private GameObject playerObj;
    private Property originProperty;
    public static GameRoot instance { get; private set; }

    /// <summary>
    /// 绑定单例并初始化
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Init()
    {

        //服务层
        GetComponent<MapManager>().Init();
        GetComponent<UIManager>().Init();

        //管理层
        GetComponent<GameManager>().Init();
        GameManager.player = playerObj.GetComponent<Player>();
        GameManager.player.position = playerPos;
    }


    // Start is called before the first frame update
    void Start()
    {
        groundLayer = transform.Find("Ground").gameObject;
        unitLayer = transform.Find("Unit").gameObject;
        playerObj = Util.Inst(playerPrefab, transform, playerPos);
        Init();
        GameManager.instance.InitGame();
        MapManager.instance.ChangeMap(originMap);
    }
}
