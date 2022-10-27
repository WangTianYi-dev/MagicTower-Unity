using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 单位属性(所有单位均有)
[Serializable]
public struct Property
{
    // 所有单位都有的数据
    public Int64 HP, ATK, DEF, Coin;
    // 钥匙
    public Int64 yellowKey, blueKey, redKey;

    public static Property Create(long HP, long ATK, long DEF, long Coin)
    {
        Property p = new Property();
        p.HP = HP;
        p.ATK = ATK;
        p.DEF = DEF;
        p.Coin = Coin;
        return p;
    }

    public override string ToString()
    {
        return $"HP: {HP}, ATK: {ATK}, DEF: {DEF}, COIN: {Coin}";
    }
}
