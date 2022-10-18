/*
 * file: ManualEntry.cs
 * author: DeamonHook
 * feature: �����ֲ�����
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ����ο�������������ʵ��
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
        HP.text = "������" + enemy.property.HP.ToString();
        ATK.text = "������" + enemy.property.ATK.ToString();
        DEF.text = "������" + enemy.property.DEF.ToString();
        coin.text = "��ң�" + enemy.property.Coin.ToString();
        enemy.RefreshDamege();
        CombatInfo info = CombatCalc.GetCombatInfo(Player.instance, enemy);
        damage.text = "�˺���" + ((info.damage != -1) ? info.damage.ToString() : "����");
        round.text = "�غ�����" + ((info.round != -1) ? info.round.ToString() : "����");
        threshold.text = "�����ٽ磺" + info.threshold.ToString();
    }
    
}
