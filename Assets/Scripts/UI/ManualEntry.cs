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

    private Text monsterName, prop, damage, round;

    // Start is called before the first frame update
    void Awake()
    {
        imgHead = transform.Find("ImgHead").GetComponent<Image>();
        monsterName = transform.Find("Name").GetComponent<Text>();
        prop = transform.Find("HP").GetComponent<Text>();
        damage = transform.Find("Damage").GetComponent<Text>();
        round = transform.Find("Round").GetComponent<Text>();
    }

    public void ShowInfo(Enemy enemy)
    {
        imgHead.sprite = enemy.gameObject.GetComponent<SpriteRenderer>().sprite;
        monsterName.text = enemy.nameInGame;
        var p = enemy.property;
        prop.text = $"生命: {p.HP} 攻击: {p.ATK} 防御: {p.DEF} 金币: {p.Coin}";
        //prop.text = "生命：" + enemy.property.HP.ToString();
        //ATK.text = "攻击：" + enemy.property.ATK.ToString();
        //DEF.text = "防御：" + enemy.property.DEF.ToString();
        //coin.text = "金币：" + enemy.property.Coin.ToString();
        enemy.RefreshDamege();
        CombatInfo info = CombatCalc.GetCombatInfo(Player.instance, enemy);
        damage.text = (info.damage != -1) ? $"估计伤害: {info.damage}" : "你无法攻击";
        //round.text = "回合数：" + ((info.round != -1) ? info.round.ToString() : "无穷");
        round.text = info.round != -1 ? $"回合数: {info.round} 攻击临界: {info.threshold}" : "";
        //threshold.text = "攻击临界：" + info.threshold.ToString();
    }
    
}
