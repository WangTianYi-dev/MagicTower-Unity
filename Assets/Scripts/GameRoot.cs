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

    public Property testProperty;

    public GameObject playerPrefab;

    private GameObject playerObj;
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
        GameManager.player.logicPos = playerPos;
        if (testProperty.HP != 0)
        {
            GameManager.player.property = testProperty;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playerObj = Util.Inst(playerPrefab, transform, playerPos);
        Init();
        GameManager.instance.InitGame();
        MapManager.instance.ChangeMap(originMap);
    }
}
