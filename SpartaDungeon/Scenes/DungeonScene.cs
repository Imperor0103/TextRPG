﻿using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Scenes
{
    public struct DungeonInfo
    {
        public string name;
        public eDifficulty difficulty;
        public float defence;
        public int reward;
        public int exp;

        public DungeonInfo(string n, eDifficulty ed, float d, int r, int e)
        {
            name = n;
            difficulty = ed;
            defence = d;
            reward = r;
            exp = e;
        }
    }
    public enum eDifficulty
    {
        EASY = 1,
        NORMAL,
        HARD
    }

    public class DungeonScene : BaseScene
    {
        // 던전이 가진 정보: 이름, 난이도, 권장 방어력, 보상, 경험치(레벨업을 위해 필요)
        Random rand;
        List<DungeonInfo> dungeonList;

        public DungeonScene(string name) : base(name)
        {
            rand = new Random();
            if (dungeonList == null)
            {
                dungeonList = new List<DungeonInfo>();
            }
            dungeonList.Add(new DungeonInfo("easy", eDifficulty.EASY, 5.0f, 1000, 10));
            dungeonList.Add(new DungeonInfo("normal", eDifficulty.NORMAL, 11.0f, 1700, 17));
            dungeonList.Add(new DungeonInfo("hard", eDifficulty.HARD, 17.0f, 2500, 25));
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
            SelectDungeon();
        }
        public void SelectDungeon()
        {
            int num;
            Console.Clear();
            Console.Write("[던전 입장]\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n\n");
            Console.WriteLine("1.쉬운 던전 | 방어력 5 이상 권장\n" +
                "2.일반 던전 | 방어력 11 이상 권장\n" +
                "3.어려운 던전 | 방어력 17 이상 권장\n" + "0.나가기\n");
            Console.Write($"원하시는 행동을 입력해주세요. \n>>");

            string input = Console.ReadLine();
            if (int.TryParse(input, out num))
            {
                switch (num)
                {
                    case 1: // 쉬운 던전
                        EnterDungeon(dungeonList[0]);
                        break;
                    case 2: // 일반 던전
                        EnterDungeon(dungeonList[1]);
                        break;
                    case 3: // 어려운 던전
                        EnterDungeon(dungeonList[2]);
                        break;
                    case 0:
                        // 씬을 교체해야한다
                        SceneManager.Instance.SetCurrentScene("town");

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
        public void EnterDungeon(DungeonInfo dungeonInfo)
        {
            Console.Clear();
            Console.Write("[던전 입장]\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n\n");

            // hp가 0이면 실패
            if (DataManager.Instance.player.playerData.hp <= 0)
            {
                // 실패
                Console.WriteLine("실패. 다시 던전 입구로 돌아갑니다. 계속하려면 Enter");
                // 체력 1로 만들고 던전입구로
                DataManager.Instance.player.playerData.hp = 1;
                Console.ReadLine();
                return;
            }
            // hp가 0보다 크다
            else
            {
                /// 공격력으로 던전 클리어시(이건... 랜덤을 따로 주는게 좋겠다. 공격력이 높을수록, 공격력으로 클리어할 가능성이 높아지게 만들자)
                // 각 던전 기본보상 + 공격력%~ 공격력*2 % 사이의 추가 보상
                // 경험치



                float damage = Math.Max(rand.Next(0, 4), rand.Next(20, 36) + dungeonInfo.defence - DataManager.Instance.player.playerData.defence);
                /// 방어력으로 던전 클리어시
                if (DataManager.Instance.player.playerData.defence < dungeonInfo.defence)
                {
                    // 1.권장 방어력보다 낮다면 60% 확률로 성공

                    // 보상 없고 체력 절반 감소
                    DataManager.Instance.player.playerData.hp /= 2; // 만약 체력이 1이면 0으로 만들어, 다음 던전 진입시 무조건 실패하게 만든다
                    // 경험치
                    DataManager.Instance.player.playerData.exp += dungeonInfo.exp;
                    CheckLevelUp();
                }
                else
                {
                    // 2.권장 방어력보다 높다면 성공
                    // 권장 방어력 +- 에 따라 종료시 체력 소모 반영
                    // 기본 체력 감소량: 20~35 랜덤
                    // 내 방어력 - 권장 방어력 만큼 랜덤 값에 추가 또는 감소 설정
                    // 경험치
                    DataManager.Instance.player.playerData.exp += dungeonInfo.exp;
                    CheckLevelUp();
                }


            }
        }
        public void CheckLevelUp()
        {
            bool isLevelUp = DataManager.Instance.player.playerData.exp >= 10 * DataManager.Instance.player.playerData.level;
            while (isLevelUp)
            {
                // 다음 레벨까지 필요한 경험치 공식: 10 * 플레이어의 현재레벨
                int remainExp = DataManager.Instance.player.playerData.exp - 10 * DataManager.Instance.player.playerData.level;
                // 레벨업
                DataManager.Instance.player.playerData.level += 1;
                DataManager.Instance.player.playerData.exp = remainExp;
                Console.WriteLine($"축하합니다. 레벨이 {DataManager.Instance.player.playerData.level}가 되었습니다");
                isLevelUp = DataManager.Instance.player.playerData.exp >= 10 * DataManager.Instance.player.playerData.level ? true : false;
            }
        }
    }
}
