using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Scenes
{
    public class SaveLoadScene : BaseScene
    {
        // 저장슬롯 5개를 배열로 선언한다
        public GameData[] dataSlots;

        public SaveLoadScene(string name) : base(name)
        {
            dataSlots = new GameData[5];
        }

        public override void Start()
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

        // 여기서 저장 로드를 선택한다
        public override void PlayGame()
        {
            Console.Clear();
            Console.WriteLine("[저장 / 불러오기] \n현재까지의 데이터를 저장하거나, 이전의 데이터를 불러올 수 있습니다.\n");
            // 저장슬롯 보여준다
            Console.WriteLine("[저장 슬롯]\n");
            // 상점의 아이템 보여주기
            for (int i = 0; i < dataSlots.Length; i++)
            {
                Console.Write($"- {i + 1} ");
                if (dataSlots[i] != null)
                {
                    // 해당 슬롯에 이미 저장 데이터가 있다면 저장 데이터를 표시
                    // 플레이어의 이름, 레벨, 무기, 갑옷, 돈 이정도만 표시하자
                }
                else
                {
                    // 없으면 비워놓는다
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("1~5 중에서 슬롯을 선택하여 저장, 불러오기, 삭제 가능\n0.이전화면으로 돌아가기\n"); // \n4.게임 저장\n5.게임 불러오기\n6.종료
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
            // 슬롯 체크한다
            int num;
            string input = Console.ReadLine();
            if (int.TryParse(input, out num))
            {
                if (num >= 1 && num <= dataSlots.Length)
                {

                }
                else if(num == 0)
                {
                    // TownScene에서 왔으면 TownScene으로, GameProcess에서 왔으면 GameProcess로 돌아가야하므로 이전 씬의 이름을 가져온다
                    SceneManager.Instance.SetCurrentScene(SceneManager.Instance.GetPrevScene().GetName());
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
        public void CheckSlot()
        {
            bool isValid = false;
            int num;
            while (!isValid)
            {
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    switch (num)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 0:
                            /// TownScene에서 왔으면 TownScene으로, GameProcess에서 왔으면 GameProcess로 돌아가야하므로 이전 씬의 이름을 가져온다
                            SceneManager.Instance.SetCurrentScene(SceneManager.Instance.GetPrevScene().GetName());
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
