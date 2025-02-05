using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class GameProcess
    {
        ItemManager itemManager;
        DataManager dataManager;
        SceneManager sceneManager;

        public bool isPlaying;

        public GameProcess()
        {
            Init();
        }
        public void Init()
        {
            // 매니저들 생성
            if (itemManager == null)
            {
                itemManager = ItemManager.Instance;
            }
            if (dataManager == null)
            {
                dataManager = DataManager.Instance;
            }
            if (sceneManager == null)
            {
                sceneManager = SceneManager.Instance;
                // 씬을 준비한다(초기 currentScene은 마을)
                sceneManager.PrepareScene();
            }
        }
        public void StartGame()
        {
            Console.Clear();
            Console.WriteLine("Text RPG \"스파르타 던전\"에 오신 여러분 환영합니다.\n");
            Console.WriteLine("1.새로 시작하기\n2.불러오기\n3.종료\n"); // \n4.게임 저장\n5.게임 불러오기\n6.종료
            Console.Write("원하시는 행동을 입력해주세요\n>> ");
            bool isValid = false;
            int num;
            while (!isValid)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    switch (num)
                    {
                        case 1:
                            CreateCharacter();
                            isValid = true;
                            break;
                        case 2:
                            LoadCharacter();
                            isValid = true;
                            break;
                        case 9:
                            isValid = true;
                            //Console.WriteLine("게임을 종료합니다");
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
        public void CreateCharacter()
        {
            // 캐릭터 생성
            Console.Clear();
            Console.WriteLine("[새로 시작하기]\n원하시는 직업을 설정해주세요.\n");
            Console.WriteLine("1.전사       2.마법사       3.궁수\n");
            bool isValid = false;
            int num;
            DataManager.Instance.player = new Player();
            DataManager.Instance.inventory = new Inventory();
            while (!isValid)
            {
                // 직업을 먼저 선택하고
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    switch (num)
                    {
                        case 1:
                            isValid = true;
                            DataManager.Instance.player.SetPlayerData(1, eClassType.WARRIOR, 10.0f, 5.0f, 100.0f, 0.0f, 15000);
                            Console.WriteLine("전사");
                            break;
                        case 2:
                            isValid = true;
                            DataManager.Instance.player.SetPlayerData(1, eClassType.MAGE, 20.0f, 1.0f, 50.0f, 50.0f, 15000);
                            Console.WriteLine("마법사");
                            break;
                        case 3:
                            isValid = true;
                            DataManager.Instance.player.SetPlayerData(1, eClassType.ARCHER, 15.0f, 3.0f, 70.0f, 70.0f, 15000);
                            Console.WriteLine("궁수");
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
            isValid = false;
            Console.WriteLine("원하시는 이름을 설정해주세요.\n");
            while (!isValid)
            {
                // 이름을 정해야겠지
                // 나중에 이름에 숫자 못들어가게 수정할 수 있으면 수정하자
                string name = Console.ReadLine();
                if (name != null)
                {
                    DataManager.Instance.player.SetPlayerName(name);
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("다시 입력하십시오.계속하려면 enter.");
                    Console.ReadLine();
                }
            }
        }
        public void LoadCharacter()
        {
            Console.WriteLine("[불러오기] 는 준비중입니다.계속하려면 enter.");
            Console.ReadLine();

            // json 파일 읽기
            return;
        }
        public void Loop()
        {
            isPlaying = true;
            while (isPlaying)
            {
                Update();
            }
        }
        public void Update()
        {
            sceneManager.Update();
        }
    }
}
