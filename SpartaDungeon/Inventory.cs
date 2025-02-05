using Newtonsoft.Json;
using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    // 플레이어가 가진 아이템은 플레이어가 아니라 인벤토리에 보관한다
    public class Inventory
    {
        public HashSet<Item> ownedItemSet;  // 플레이어 소유 아이템
        public List<Item> armedItemList;    // 장착한 아이템


        // 아이템 장착 관련
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]   /// null이어도 저장할 때 제거되지 않음
        public Item? weapon;
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public Item? armor;

        public Item Weapon
        {
            get { return weapon; }
            set { weapon = value; } // value가 null이면 null 할당
        }
        public Item Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        public Inventory()
        {
            if (ownedItemSet == null)
            {
                ownedItemSet = new HashSet<Item>();
            }
            if (armedItemList == null)
            {
                armedItemList = new List<Item>();
            }
        }

        public void ShowInventory()
        {
            bool isValid = false;
            int num;
            while (!isValid)
            {
                Console.Clear();
                // 내가 가지고 있는 아이템 목록을 보여준다
                Console.WriteLine("[인벤토리]\n보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]\n");

                ItemManager.Instance.ShowPlayerItem();

                Console.WriteLine("1.장착 관리\n0.나가기\n"); //
                Console.Write("원하시는 행동을 입력해주세요\n>> ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    switch (num)
                    {
                        case 1:
                            // 장착 관리
                            DataManager.Instance.inventory.ManageEquipment();
                            break;
                        case 0:
                            isValid = true;
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                }
            }

        }

        public void ManageEquipment()
        {
            // 장착관리
            // 인덱스 접근하기 위해 HashSet 있는 아이템을 List에 저장해야한다
            //List<Item> itemList = new List<Item>();
            //foreach (var item in ownedItemSet)
            //{
            //    itemList.Add(item);
            //}
            bool isValid = false;
            int num;
            while (!isValid)
            {
                List<Item> itemList = ownedItemSet.ToList(); // 한줄에 끝나니깐 너무 허무하다.
                Console.Clear();
                Console.WriteLine("[인벤토리]\n보유 중인 아이템을 관리할 수 있습니다. \n");
                Console.WriteLine("[아이템 목록]\n");

                for (int i = 0; i < itemList.Count; i++)
                {
                    Console.Write($"- {i + 1} ");
                    // 해당 아이템이 armedList에도 있다면
                    if (armedItemList.Contains(itemList[i]))
                    {
                        Console.Write("[E] ");
                    }
                    Console.Write($"{itemList[i].itemData.name} | ");
                    eClassType tmpClassType = itemList[i].itemData.classType & eClassType.ALL;
                    switch (tmpClassType)
                    {
                        case eClassType.WARRIOR:
                            Console.Write("전사 전용 | ");
                            break;
                        case eClassType.MAGE:
                            Console.Write("마법사 전용 | ");
                            break;
                        case eClassType.ARCHER:
                            Console.Write("궁수 전용 | ");
                            break;
                        case eClassType.ALL:
                            Console.Write("전직업 공용 | ");
                            break;
                        default:
                            Console.Write("장착 불가능한 아이템 | ");
                            break;
                    }
                    eItemType tmpItemType = itemList[i].itemData.itemType;
                    switch (tmpItemType)
                    {
                        case eItemType.WEAPON:
                            Console.Write($"공격력 +{itemList[i].itemData.attack} | ");
                            break;
                        case eItemType.ARMOR:
                            Console.Write($"방어력 +{itemList[i].itemData.defence} | ");
                            break;
                        default:
                            Console.Write("아이템 타입 재설정이 필요합니다");
                            break;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.Write($"0. 나가기 \n\n");
                Console.Write($"장착하고싶은 아이템의 번호를 입력해주세요. \n>>");
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    if (num >= 1 && num <= itemList.Count)
                    {
                        EquipItem(itemList[num - 1]);
                    }
                    else if (num == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                }
            }
        }
        public void EquipItem(Item item)
        {
            // 플레이어의 장비 장착
            // armor 또는 weapon 이고
            // 맞는 직업이거나 전 직업 공용 아이템
            if (item.itemData.itemType == eItemType.WEAPON ||
                item.itemData.itemType == eItemType.ARMOR)
            {
                if (item.itemData.classType == DataManager.Instance.player.playerData.charaClass ||
                    item.itemData.classType == eClassType.ALL)
                {
                    // 기존 아이템 해제
                    if (item.itemData.itemType == eItemType.WEAPON)
                    {
                        if (DataManager.Instance.inventory.armedItemList.Contains(item))
                        {
                            // 선택한 무기를 이미 장착중이라면 해제만 한다
                            armedItemList.Remove(weapon);
                            Console.WriteLine($"{weapon.itemData.name}을 해제했습니다.계속 진행하려면 Enter.");
                            weapon = null;  // 해제
                            Console.ReadLine();
                            return;
                        }
                        else
                        {
                            if (weapon != null) // 기존 무기가 있다면
                            {
                                armedItemList.Remove(weapon);
                                Console.WriteLine($"{weapon.itemData.name}을 해제했습니다.");
                                weapon = null;  // 해제
                            }
                            // 없거나 기존무기를 해제한 다음
                            weapon = item;  // 새 아이템 장착(능력치 갱신은 공격, 방어할 때만)
                            armedItemList.Add(weapon);
                            Console.WriteLine($"{weapon.itemData.name}을 장착했습니다.계속 진행하려면 Enter.");
                            Console.ReadLine();
                            return;
                        }
                    }
                    else if (item.itemData.itemType == eItemType.ARMOR)
                    {
                        if (DataManager.Instance.inventory.armedItemList.Contains(item))
                        {
                            // 선택한 갑옷을 이미 장착중이라면 해제만 한다
                            armedItemList.Remove(armor);
                            Console.WriteLine($"{armor.itemData.name}을 해제했습니다. 계속 진행하려면 Enter.");
                            armor = null;
                            Console.ReadLine();
                            return;
                        }
                        else
                        {
                            if (armor != null) // 기존 방어구가 있다면
                            {
                                armedItemList.Remove(armor);
                                Console.WriteLine($"{armor.itemData.name}을 해제했습니다.");
                                armor = null;
                            }
                            // 없거나 기존갑옷을 해제한 다음
                            armor = item;
                            armedItemList.Add(armor);
                            Console.WriteLine($"{armor.itemData.name}을 장착했습니다.계속 진행하려면 Enter.");
                            Console.ReadLine();
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"장착할 수 없습니다. {item.itemData.classType} 전용 아이템입니다. 계속 진행하려면 Enter.");
                    Console.ReadLine();
                    return;
                }
            }
        }
        //public void UnEquipItem(int num)
        //{
        //    // 장비를 해제하고 싶다(맨손으로 싸우고 싶을 때)
        //    // 플레이어가 입력한 숫자에 따라 달라진다
        //    if (num == 1)
        //    {
        //        // 무기 해제
        //        if (weapon != null)
        //        {
        //            Console.WriteLine($"{weapon.itemData.name}을 해제했습니다");
        //            weapon = null;
        //        }
        //    }
        //    else if (num == 2)
        //    {
        //        // 방어구 해제
        //        if (armor != null)
        //        {
        //            Console.WriteLine($"{armor.itemData.name}을 해제했습니다");
        //            armor = null;
        //        }
        //    }
        //}

        public bool IsWeaponEquiped()
        {
            // weapon가 null 아니라면 할당된 것이다
            return weapon != null ? true : false;
        }
        public bool IsArmorEquiped()
        {
            // armor가 null 아니라면 할당된 것이다
            return armor != null ? true : false;
        }
    }
}
