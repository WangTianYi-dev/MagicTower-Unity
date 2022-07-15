using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 单位属性(所有单位共享)
public class Property
{
    // 所有单位都有的数据
    public Int64 HP, ATK, DEF, Coin;
    // 钥匙
    public int yellowKey, blueKey, redKey;

    // 物品
    Dictionary<String, int> items;

    HashSet<String> specials;

    public Property Clone(Property p) 
    {
        Property np = (Property)MemberwiseClone();
        return np;
    }

    // 目前未使用
    public int exp;

    /// <summary>
    /// 根据名字返回当前物品数量
    /// </summary>
    /// <param name="name">物品名字</param>
    /// <returns>物品数量</returns>
    public int CountItem(String name)
    {
        int count;
        if (items.TryGetValue(name, out count))
        {
            return count;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 改变物品数量
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void ChangeItem(String name, int value)
    {
        if (items.ContainsKey(name))
        {
            items[name] += value;
        }
        else
        {
            items.Add(name, value);
        }
    }
}