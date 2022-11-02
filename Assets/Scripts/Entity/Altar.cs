/*
 * file: Store.cs
 * author: DeamonHook
 * feature: 不会涨价的商店
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class Altar : Store
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 
        this.passable = false;
        setting["type"] = "altar";
    }

    /// <summary>
    /// 下一次购买价格
    /// 目前为 10 * (n+1) * n + 20 （n为购买次数，从0开始）
    /// </summary>
    public long nextPrice
    {
        get
        {
            var altarCount = GameManager.instance.altarCount;
            return
                10 * (altarCount + 1) * altarCount + 20;
            
        }
    }

    /// <summary>
    /// 下一次加成效果
    /// 目前为n * 4 + 2（n为购买次数，从0开始）
    /// </summary>
    public long nextAddition
    {
        get
        {
            var altarCount = GameManager.instance.altarCount;
            return altarCount * 4 + 2;
        }
    }


    /// <summary>
    /// 提示信息
    /// </summary>
    public string message
    {
        get
        {
            return $"贪婪之神：\n又一个勇者吗？如果需要我的力量，只需奉献出你的血肉、灵魂，或者……金币\n" +
                $"需要{nextPrice}金币以换取：\n" +
                $"1. {nextAddition*100}点生命\n" +
                $"2. {nextAddition}点攻击\n" +
                $"3. {nextAddition}点防御";
        }
    }

    
    public List<Func<bool>> transactions
    {
        get
        {
            return new List<Func<bool>>
            {
                ()=>
                { 
                    var tran = 
                        GameManager.instance.CreateTransaction(
                            "coin", nextPrice.ToString(), "hp", 
                            (nextAddition*100).ToString());
                    bool res = false;
                    if (tran())
                    {
                        GameManager.instance.altarCount += 1;
                        res = true;
                    }
                    StoreWindow.instance.Refresh(message, transactions);
                    return res;
                },
                ()=>
                {
                    var tran = GameManager.instance.CreateTransaction("coin", nextPrice.ToString(), "atk", nextAddition.ToString());
                    bool res = false;
                    if (tran())
                    {
                        GameManager.instance.altarCount += 1;
                        res = true;
                    }
                    StoreWindow.instance.Refresh(message, transactions);
                    return res;
                },
                ()=>
                {
                    var tran = GameManager.instance.CreateTransaction("coin", nextPrice.ToString(), "def", nextAddition.ToString());
                    bool res = false;
                    if (tran())
                    {
                        GameManager.instance.altarCount += 1;
                        res = true;
                    }
                    StoreWindow.instance.Refresh(message, transactions);
                    return res;
                }
            };
        }
    }


    public override void BeforeCollision()
    {
        base.BeforeCollision();
    }

    public override void OnTriggerDone()
    {
        base.OnTriggerDone();
    }
}
