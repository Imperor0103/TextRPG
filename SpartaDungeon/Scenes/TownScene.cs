using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Scenes
{
    public class TownScene : BaseScene
    {
        public TownScene(string name) : base(name)
        {
        }

        public override void LoadScene()
        {
            // 생성자를 안썼다면, 여기에서 Awake를 호출하여 초기화를 했을 것임
        }
        public override void UnloadScene()
        {
        }
        public override void Start()
        {
        }

        public override void Update()
        {
            PlayGame();
        }
        public override void PlayGame()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다. \n이 곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1.상태 보기\n2.인벤토리\n3.상점\n4.던전입장\n5.휴식하기\n6.저장하기\n7.시작화면으로\n"); // \n4.게임 저장\n5.게임 불러오기\n6.종료
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
            int num;
            string input = Console.ReadLine();
            if (int.TryParse(input, out num))
            {
                switch (num)
                {
                    case 1:
                        ShowStatus();
                        break;
                    case 2:
                        DataManager.Instance.inventory.ShowInventory();
                        break;
                    case 3:
                        // currentScene을 storeScene으로 변경
                        SceneManager.Instance.SetCurrentScene("store");
                        break;
                    case 4:
                        SceneManager.Instance.SetCurrentScene("dungeon");
                        break;
                    case 5:
                        Rest();
                        break;
                    case 6:
                        SceneManager.Instance.SetCurrentScene("saveLoad");
                        break;
                    case 7:
                        SceneManager.Instance.SetCurrentScene("entry");
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                        Console.ReadLine();
                        break;
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                Console.ReadLine();
            }
        }
        public void ShowStatus()
        {
            Console.Clear();
            //string message = string.Format($"[상태 보기]\n캐릭터의 정보가 표시됩니다.\n\n" +
            //    $"Lv. {DataManager.Instance.player.playerData.level}\n" +
            //    $"{DataManager.Instance.player.playerData.name} ({DataManager.Instance.player.playerData.charaClass})\n" +
            //    $"공격력 : {DataManager.Instance.player.playerData.attack}\n" +
            //    $"방어력 : {DataManager.Instance.player.playerData.defence}\n" +
            //    $"체 력 : {DataManager.Instance.player.playerData.hp}\n" +
            //    $"Gold : {DataManager.Instance.player.playerData.gold} G\n\n" +
            //    $"0. 나가기\n\n" +
            //    $"원하시는 행동을 입력해주세요.\n>>");
            //Console.WriteLine(message);
            Console.Write($"[상태 보기]\n캐릭터의 정보가 표시됩니다.\n\n");
            Console.Write($"Lv. {DataManager.Instance.player.playerData.level} \n");
            Console.Write($"{DataManager.Instance.player.playerData.name} ({DataManager.Instance.player.playerData.charaClass}) \n");
            //
            Console.Write($"공격력 : {DataManager.Instance.player.playerData.attack} ");
            /// 아이템 공격력 추가
            if (DataManager.Instance.inventory.weapon != null)
            {
                Console.Write($"(+{DataManager.Instance.inventory.weapon.itemData.attack}) ");
            }
            Console.Write("\n");
            //
            Console.Write($"방어력 : {DataManager.Instance.player.playerData.defence} ");
            /// 아이템 방어력 추가
            if (DataManager.Instance.inventory.armor != null)
            {
                Console.Write($"(+{DataManager.Instance.inventory.armor.itemData.defence}) ");
            }
            Console.Write("\n");
            //
            Console.Write($"체 력 : {DataManager.Instance.player.playerData.hp} / {DataManager.Instance.player.playerData.maxHp} \n");
            Console.Write($"exp : {DataManager.Instance.player.playerData.exp} / 다음레벨까지 남은 경험치: {10 * DataManager.Instance.player.playerData.level - DataManager.Instance.player.playerData.exp} \n");
            Console.Write($"Gold : {DataManager.Instance.player.playerData.gold} G\n\n");
            Console.Write($"0. 나가기 \n\n");
            Console.Write($"원하시는 행동을 입력해주세요. \n>>");
            bool isValid = false;
            int num;
            while (!isValid)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    switch (num)
                    {
                        case 0:
                            isValid = true;
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                    Console.ReadLine();
                }
            }
        }

        public void Rest()
        {
            bool isValid = false;
            int num;
            while (!isValid)
            {
                Console.Clear();
                Console.Write($"[휴식하기]\n500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드: {DataManager.Instance.player.playerData.gold} G)\n\n");
                Console.WriteLine("1.휴식하기\n0.나가기\n");
                Console.Write($"원하시는 행동을 입력해주세요. \n>>");
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    switch (num)
                    {
                        case 1:
                            // 체력을 100 회복하며 최대치를 넘어가지않는다
                            if (DataManager.Instance.player.playerData.hp < DataManager.Instance.player.playerData.maxHp)
                            {
                                DataManager.Instance.player.playerData.gold -= 500;
                                Console.WriteLine("500 G 를 지불했습니다");
                                DataManager.Instance.player.playerData.hp =
                                    Math.Min(DataManager.Instance.player.playerData.hp + 100, DataManager.Instance.player.playerData.maxHp);
                                Console.WriteLine("체력 100이 회복되었습니다. 계속 하려면 Enter.");
                                Console.ReadLine();
                            }
                            break;
                        case 0:
                            isValid = true;
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                            Console.ReadLine();
                            break;
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
