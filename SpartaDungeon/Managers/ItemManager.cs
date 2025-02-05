using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Managers
{
    // 아이템 매니저가 상점의 역할도 한다
    public class ItemManager : Singleton<ItemManager>
    {
        // 모든 아이템을 관리한다
        public List<Item> itemList;
        public ItemManager()
        {
            if (itemList == null)
            {
                itemList = new List<Item>();
            }
            CreateItem("수련자 갑옷", eItemType.ARMOR, eClassType.ALL, 0f, 5f, 0f, "수련에 도움을 주는 갑옷입니다.", 1000);
            CreateItem("무쇠갑옷", eItemType.ARMOR, eClassType.ALL, 0f, 9f, 0f, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500);
            CreateItem("스파르타의 갑옷", eItemType.ARMOR, eClassType.WARRIOR, 0f, 15f, 0f, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            CreateItem("황혼의 도포", eItemType.ARMOR, eClassType.MAGE, 0f, 15f, 0f, "바람이 많이 부는 나라에서 가져온 마법사 전용 전설의 갑옷입니다.", 3500);
            CreateItem("인내", eItemType.ARMOR, eClassType.ARCHER, 0f, 15f, 0f, "아마존에서 가져온 궁수 전용 전설의 갑옷입니다.", 3500);


            CreateItem("낡은 검", eItemType.WEAPON, eClassType.ALL, 2f, 0f, 0f, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            CreateItem("청동 도끼", eItemType.WEAPON, eClassType.ALL, 5f, 0f, 0f, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            CreateItem("스파르타의 창", eItemType.WEAPON, eClassType.WARRIOR, 7f, 0f, 0f, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1500);
            CreateItem("초승달", eItemType.WEAPON, eClassType.MAGE, 7f, 0f, 0f, "어느 사막에서 가져온 전설의 스태프입니다.", 1500);
            CreateItem("신뢰", eItemType.WEAPON, eClassType.ARCHER, 7f, 0f, 0f, "아마존에서 가져온 전설의 활입니다.", 1500);
        }
        public void CreateItem(string name, eItemType itemType, eClassType classType, float att, float def, float h, string des, int pr)
        {
            Item newItem = new Item();
            newItem.SetData(name, itemType, classType, att, def, h, des, pr);
            itemList.Add(newItem);
        }
        public void ShowPlayerItem()
        {
            // 아이템 보여주기
            // 장착 중인 아이템 앞에 [E] 표시
            // 플레이어아이템
            foreach (Item item in DataManager.Instance.inventory.ownedItemSet)
            {
                Console.Write($"- {item.itemData.name} |");
                eClassType tmpClassType = item.itemData.classType & eClassType.ALL;
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
                eItemType tmpItemType = item.itemData.itemType;
                switch (tmpItemType)
                {
                    case eItemType.WEAPON:
                        Console.Write($"공격력 +{item.itemData.attack} | ");
                        break;
                    case eItemType.ARMOR:
                        Console.Write($"방어력 +{item.itemData.defence} |");
                        break;
                    default:
                        Console.Write("아이템 타입 재설정이 필요합니다");
                        break;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void ShowStoreItem()
        {
            Console.Clear();
            // 상점에서는 거래
            Console.WriteLine("[상점]\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine($"[보유 골드]\n{DataManager.Instance.player.playerData.gold} G\n");
            Console.WriteLine("[아이템 목록]\n");

            // 남은 목록을 보여주는데, 구매를 하면, 지워져야 한다는거다
            // 그래서 반복문을 돌 것이며,
            foreach (Item item in ItemManager.Instance.itemList)
            {
                Console.Write($"- {item.itemData.name} |");
                eClassType tmpClassType = item.itemData.classType & eClassType.ALL;
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
                eItemType tmpItemType = item.itemData.itemType;
                switch (tmpItemType)
                {
                    case eItemType.WEAPON:
                        Console.Write($"공격력 +{item.itemData.attack} | ");
                        break;
                    case eItemType.ARMOR:
                        Console.Write($"방어력 +{item.itemData.defence} | ");
                        break;
                    default:
                        Console.Write("아이템 타입 재설정이 필요합니다");
                        break;
                }
                Console.Write($"{item.itemData.description} | ");
                // 플레이어가 가지고 있는 것은 구매완료로 뜨게 만들어야함
                bool isExist = DataManager.Instance.inventory.ownedItemSet.Contains<Item>(item);
                if (isExist)
                {
                    Console.Write("구매완료");
                }
                else
                {
                    Console.Write($"{item.itemData.price} G");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void BuyItem()
        {
            bool isValid = false;
            int num;
            while (!isValid)
            {
                // 플레이어의 구매
                List<Item> itemList = ItemManager.Instance.itemList.ToList(); // 한줄에 끝나니깐 너무 허무하다.

                // 아이템 매니저(상점)이 가진 아이템을 출력해서 보여준다
                Console.Clear();
                Console.WriteLine("[상점]\n필요한 아이템을 얻을 수 있는 상점입니다. \n");
                Console.WriteLine($"[보유 골드]\n{DataManager.Instance.player.playerData.gold} G\n");
                Console.WriteLine("[아이템 목록]\n");
                // 상점의 아이템 보여주기
                for (int i = 0; i < itemList.Count; i++)
                {
                    Console.Write($"- {i + 1} ");
                    // 해당 아이템이 armedList에도 있다면
                    if (DataManager.Instance.inventory.armedItemList.Contains(itemList[i]))
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
                            Console.Write("아이템 타입을 설정해주세요."); /// 디버그용 메세지
                            break;
                    }
                    Console.Write($"{itemList[i].itemData.description} | ");
                    bool isExist = DataManager.Instance.inventory.ownedItemSet.Contains<Item>(itemList[i]);
                    if (isExist)
                    {
                        Console.Write("구매완료");
                    }
                    else
                    {
                        Console.Write($"{itemList[i].itemData.price} G");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine("0.나가기\n"); //

                Console.Write("구매하고 싶은 아이템의 번호를 입력해주세요.\n>> ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    if (num >= 1 && num <= itemList.Count)
                    {
                        // 입력한 아이템을 받아와야한다
                        Item shopItem = itemList[num - 1];
                        // 플레이어가 가지고 있는 것은 재구매 불가
                        bool isExist = DataManager.Instance.inventory.ownedItemSet.Contains<Item>(shopItem);
                        if (isExist)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다. 계속하려면 enter.");
                        }
                        else
                        {
                            // 소지금이 가격 이상이어야 구매가능
                            if (DataManager.Instance.player.playerData.gold >= shopItem.itemData.price)
                            {
                                DataManager.Instance.player.playerData.gold -= shopItem.itemData.price;
                                // 플레이어가 구매한 아이템은 플레이어의 인벤토리에 추가
                                DataManager.Instance.inventory.ownedItemSet.Add(shopItem);
                                Console.WriteLine($"{shopItem.itemData.name}을 {shopItem.itemData.price} Gold로 구매했습니다. 계속하려면 enter.");
                            }
                            else
                            {
                                Console.WriteLine("소지금이 부족합니다. 계속하려면 enter.");
                            }
                        }
                        Console.ReadLine();
                    }
                    else if (num == 0)
                    {
                        isValid = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                    Console.ReadLine();
                }
            }
        }
        public void SellItem()
        {
            bool isValid = false;
            int num;
            while (!isValid)
            {
                List<Item> itemList = DataManager.Instance.inventory.ownedItemSet.ToList(); // 한줄에 끝나니깐 너무 허무하다.
                Console.Clear();
                Console.WriteLine("[상점]\n필요한 아이템을 얻을 수 있는 상점입니다. \n");
                Console.WriteLine($"[보유 골드]\n{DataManager.Instance.player.playerData.gold} G\n");
                Console.WriteLine("[아이템 목록]\n");
                // 플레이어의 아이템 보여주기
                for (int i = 0; i < itemList.Count; i++)
                {
                    Console.Write($"- {i + 1} ");
                    // 해당 아이템이 armedList에도 있다면
                    if (DataManager.Instance.inventory.armedItemList.Contains(itemList[i]))
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
                            Console.Write("아이템 타입을 설정해주세요."); /// 디버그용 메세지
                            break;
                    }
                    Console.Write($"{itemList[i].itemData.description} | ");
                    Console.Write($"판매가격: {itemList[i].itemData.price * 85 / 100} G\n");
                }
                Console.WriteLine();
                Console.WriteLine("0.나가기\n"); //

                Console.Write("판매하고 싶은 아이템의 번호를 입력해주세요.\n>> ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    if (num >= 1 && num <= itemList.Count)
                    {
                        Item playerItem = itemList[num - 1];
                        // 여기서 입력 받아야할듯
                        // 장착중인 아이템은 판매불가
                        bool isExist = DataManager.Instance.inventory.armedItemList.Contains<Item>(playerItem);
                        if (isExist)
                        {
                            Console.WriteLine("장착중인 아이템은 판매가 불가능합니다.");
                        }
                        else
                        {
                            // 상점의 돈은 무한하니까 그냥 팔면 된다
                            // 상점은 모든 아이템을 다 살 수 있다
                            DataManager.Instance.player.playerData.gold += playerItem.itemData.price * 85 / 100;
                            DataManager.Instance.inventory.ownedItemSet.Remove(playerItem);
                            Console.WriteLine($"{playerItem.itemData.name}을 팔고 {playerItem.itemData.price * 85 / 100} Gold를 받았습니다. 계속하려면 enter.");
                        }
                        Console.ReadLine();
                    }
                    else if (num == 0)
                    {
                        isValid = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                    Console.ReadLine();
                }
            }
        }
    }
}