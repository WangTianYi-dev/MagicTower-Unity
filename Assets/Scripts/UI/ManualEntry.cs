/*
 * file: ManualEntry.cs
 * author: DeamonHook
 * feature: 怪物手册子项
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 这里参考了橘生淮北的实现
public class ManualEntry : MonoBehaviour
{
    private Image imgHead;

    private Text monsterName, HP, ATK, DEF, coin, damage, round, threshold, special;

    // Start is called before the first frame update
    void Awake()
    {
        imgHead = transform.Find("ImgHead").GetComponent<Image>();
        monsterName = transform.Find("Name").GetComponent<Text>();
        HP = transform.Find("HP").GetComponent<Text>();
        ATK = transform.Find("ATK").GetComponent<Text>();
        DEF = transform.Find("DEF").GetComponent<Text>();
        coin = transform.Find("Coin").GetComponent<Text>();
        damage = transform.Find("Damage").GetComponent<Text>();
        round = transform.Find("Round").GetComponent<Text>();
        threshold = transform.Find("ATKthreshold").GetComponent<Text>();
        special = transform.Find("Special").GetComponent<Text>();
    }

    public void ShowInfo(Enemy enemy)
    {
        imgHead.sprite = enemy.gameObject.GetComponent<SpriteRenderer>().sprite;
        monsterName.text = enemy.nameInGame;
        HP.text = "生命：" + enemy.property.HP.ToString();
        ATK.text = "攻击：" + enemy.property.ATK.ToString();
        DEF.text = "防御：" + enemy.property.DEF.ToString();
        coin.text = "金币：" + enemy.property.Coin.ToString();
        enemy.RefreshDamege();
        CombatInfo info = CombatCalc.GetCombatInfo(Player.instance, enemy);
        damage.text = "伤害：" + ((info.damage != -1) ? info.damage.ToString() : "无穷");
        round.text = "回合数：" + ((info.round != -1) ? info.round.ToString() : "无穷");
        threshold.text = "攻击临界：" + info.threshold.ToString();
    }
    
}
