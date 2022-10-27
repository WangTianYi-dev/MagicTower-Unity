/*
 * file: Store.cs
 * author: DeamonHook
 * feature: �����Ǽ۵��̵�
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
        this.triggerType = this.triggerType == "" ? "altar" : this.triggerType;
    }

    /// <summary>
    /// ��һ�ι���۸�
    /// ĿǰΪ 10 * (n+1) * n + 20 ��nΪ�����������0��ʼ��
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
    /// ��һ�μӳ�Ч��
    /// ĿǰΪn * 4 + 2��nΪ�����������0��ʼ��
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
    /// ��ʾ��Ϣ
    /// </summary>
    public string message
    {
        get
        {
            return $"̰��֮��\n��һ�������������Ҫ�ҵ�������ֻ����׳����Ѫ�⡢��꣬���ߡ������\n" +
                $"��Ҫ{nextPrice}����Ի�ȡ��\n" +
                $"1. {nextAddition*100}������\n" +
                $"2. {nextAddition}�㹥��\n" +
                $"3. {nextAddition}�����";
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
                    if (tran())
                    {
                        GameManager.instance.altarCount += 1;
                        return true;
                    }
                    return false;
                },
                ()=>
                {
                    var tran = GameManager.instance.CreateTransaction("coin", nextPrice.ToString(), "atk", nextAddition.ToString());
                    if (tran())
                    {
                        GameManager.instance.altarCount += 1;
                        return true;
                    }
                    return false;
                },
                ()=>
                {
                    var tran = GameManager.instance.CreateTransaction("coin", nextPrice.ToString(), "def", nextAddition.ToString());
                    if (tran())
                    {
                        GameManager.instance.altarCount += 1;
                        return true;
                    }
                    return false;
                }
            };
        }
    }


    public override void BeforeCollision()
    {
        base.BeforeCollision();
        GameManager.instance.TriggerByEntity(this);
    }

    public override void OnTriggerDone()
    {
        base.OnTriggerDone();
        //GameManager.instance.altarCount += 1;
        //print($"altarcount: {GameManager.instance.altarCount}");
        StoreWindow.instance.Refresh(message, transactions);
    }
}
