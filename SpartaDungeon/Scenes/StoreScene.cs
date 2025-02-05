using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Scenes
{
    public class StoreScene : BaseScene
    {
        public StoreScene(string name) : base(name)
        {
        }
        public override void LoadScene()
        {
        }
        public override void UnloadScene()
        {
        }

        public override void Update()
        {
            PlayGame();
        }
        public override void PlayGame()
        {
            ItemManager.Instance.ShowStoreItem();
            // 입력 받아라
            Console.WriteLine("1.아이템 구매\n2.아이템 판매\n0.나가기\n"); //
            Console.Write("원하시는 행동을 입력해주세요\n>> ");

            int num;
            string input = Console.ReadLine();
            if (int.TryParse(input, out num))
            {
                switch (num)
                {
                    case 1:
                        // 아이템 구매
                        ItemManager.Instance.BuyItem();
                        break;
                    case 2:
                        // 아이템 판매
                        ItemManager.Instance.SellItem();
                        break;
                    case 0:
                        // 씬을 교체해야한다
                        SceneManager.Instance.SetCurrentScene("town");
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
