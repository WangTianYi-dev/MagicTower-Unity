/*
 * file: GameRoot.cs
 * author: DeamonHook
 * date: 7/6/2022
 * feature: 游戏的入口
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [Header("生成地图的层数")]
    public int floor;
    [Header("生成主角的位置")]
    public Vector2Int playerPos;


    public static GameRoot instance { get; private set; }

    /// <summary>
    /// 绑定单例并初始化
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Init()
    {
        //资源层
        GetComponent<ResResolver>().Init();

        //服务层
        GetComponent<MapManager>().Init();
        
        //管理层
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
