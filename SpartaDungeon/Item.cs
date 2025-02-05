using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public struct ItemData
    {
        public string name;
        public eItemType itemType;
        public eClassType classType;    // ALL: 모두 장착가능
        public float attack;
        public float defence;
        public float hp;
        public string description;
        public int price;
    }

    public enum eItemType
    {
        WEAPON = 1,
        ARMOR = 2,
    }

    public class Item
    {
        public ItemData itemData;
        public Item()
        {
        }
        public void SetData(string name, eItemType itemType, eClassType classType, float att, float def, float h, string des, int pr)
        {
            itemData.name = name;
            itemData.itemType = itemType;
            itemData.classType = classType;
            itemData.attack = att;
            itemData.defence = def;
            itemData.hp = h;
            itemData.description = des;
            itemData.price = pr;
        }
    }
}
