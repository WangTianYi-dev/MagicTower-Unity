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
        prop.text = $"����: {p.HP} ����: {p.ATK} ����: {p.DEF} ���: {p.Coin}";
        //prop.text = "������" + enemy.property.HP.ToString();
        //ATK.text = "������" + enemy.property.ATK.ToString();
        //DEF.text = "������" + enemy.property.DEF.ToString();
        //coin.text = "��ң�" + enemy.property.Coin.ToString();
        enemy.RefreshDamege();
        CombatInfo info = CombatCalc.GetCombatInfo(Player.instance, enemy);
        damage.text = (info.damage != -1) ? $"�����˺�: {info.damage}" : "���޷�����";
        //round.text = "�غ�����" + ((info.round != -1) ? info.round.ToString() : "����");
        round.text = info.round != -1 ? $"�غ���: {info.round} �����ٽ�: {info.threshold}" : "";
        //threshold.text = "�����ٽ磺" + info.threshold.ToString();
    }
    
}
