using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        Random rand = new Random();     // 멤버로 선언하면서 동시에 초기화하면 처음 1번만 초기화되므로, 비슷한 숫자들이 반복할 가능성이 줄어듦
        List<DungeonInfo> dungeonList;

        public DungeonScene(string name) : base(name)
        {
            //rand = new Random();
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
            if (DataManager.Instance.player.playerData.hp <= 0.0f)
            {
                // 실패
                Console.WriteLine("실패. 체력이 0입니다. 다시 던전 입구로 돌아갑니다. 계속하려면 Enter");
                // 체력 1로 만들고 던전입구로
                DataManager.Instance.player.playerData.hp = 1;
                Console.ReadLine();
                return;
            }
            // hp가 0보다 크다
            else
            {
                int chance1 = rand.Next(0, 100);
                int rewardGold;
                float playerAttack = DataManager.Instance.player.PlayerAttack();
                float playerDefence = DataManager.Instance.player.PlayerDefence();
                float damage = Math.Max((float)rand.Next(0, 10), (float)rand.Next(20, 36) + dungeonInfo.defence - playerDefence);

                /// 공격력으로 던전 클리어시(공격력이 높을수록, 공격력으로 클리어할 가능성이 높아진다)
                if (chance1 < 20 + playerAttack)
                {
                    /// 공격력으로 던전 클리어시 데미지를 입지 않는다                    
                    Console.WriteLine($"체력: {DataManager.Instance.player.playerData.hp} -> {DataManager.Instance.player.playerData.hp}");
                    // 각 던전 기본보상 + 공격력%~ 공격력*2 % 사이의 추가 보상
                    int temp = DataManager.Instance.player.playerData.gold;
                    int extra = (dungeonInfo.reward * (int)(playerAttack) / 100);
                    rewardGold = dungeonInfo.reward + rand.Next(extra, extra * 2);
                    DataManager.Instance.player.playerData.gold += dungeonInfo.reward;
                    Console.WriteLine($"Gold: {temp} G -> {DataManager.Instance.player.playerData.gold} G");
                    // 경험치
                    DataManager.Instance.player.playerData.exp += dungeonInfo.exp;
                    Console.WriteLine($"{dungeonInfo.exp} 경험치를 얻었습니다.");
                    CheckLevelUp();
                    Console.WriteLine();
                    Console.WriteLine($"{dungeonInfo.name} 던전을 공략했습니다!. 던전 입구로 돌아갑니다. 계속하려면 Enter");
                    Console.ReadLine();
                }
                else
                {
                    /// 방어력으로 던전 클리어
                    if (playerDefence < dungeonInfo.defence)
                    {
                        int chance2 = rand.Next(0, 100);
                        // 1.권장 방어력보다 낮다면 40% 확률로 실패
                        if (chance2 < 40) // 실패
                        {
                            // 보상 없고 체력 절반 감소
                            float tempHp = DataManager.Instance.player.playerData.hp;
                            DataManager.Instance.player.playerData.hp /= 2; // 만약 체력이 1이면 0으로 만들어, 다음 던전 진입시 무조건 실패하게 만든다
                            Console.WriteLine($"체력: {tempHp} -> {DataManager.Instance.player.playerData.hp}");
                            Console.WriteLine();
                            Console.WriteLine("실패. 다시 던전 입구로 돌아갑니다. 계속하려면 Enter");
                            Console.ReadLine();
                            return;
                        }
                        else /// 주의: 데미지를 받고 hp가 0이 되면 공략실패, 그 외는 성공
                        {
                            if (DataManager.Instance.player.playerData.hp - damage <= 0)
                            {
                                // 실패
                                Console.WriteLine("실패. 체력이 0입니다. 다시 던전 입구로 돌아갑니다. 계속하려면 Enter");
                                // 체력 1로 만들고 던전입구로
                                DataManager.Instance.player.playerData.hp = 1;
                                Console.ReadLine();
                                return;
                            }
                            else
                            {
                                // 데미지
                                float tempHp = DataManager.Instance.player.playerData.hp;
                                DataManager.Instance.player.playerData.hp -= damage;
                                Console.WriteLine($"체력: {tempHp} -> {DataManager.Instance.player.playerData.hp}");
                                // 기본보상 클리어
                                int tempGold = DataManager.Instance.player.playerData.gold;
                                DataManager.Instance.player.playerData.gold += dungeonInfo.reward;
                                Console.WriteLine($"Gold: {tempGold} G -> {DataManager.Instance.player.playerData.gold} G");
                                // 경험치
                                DataManager.Instance.player.playerData.exp += dungeonInfo.exp;
                                Console.WriteLine($"{dungeonInfo.exp} 경험치를 얻었습니다.");
                                CheckLevelUp();
                                Console.WriteLine();
                                Console.WriteLine($"{dungeonInfo.name} 던전을 공략했습니다!. 던전 입구로 돌아갑니다. 계속하려면 Enter");
                                Console.ReadLine();
                            }
                        }
                    }
                    else /// 주의: 데미지를 받고 hp가 0이 되면 공략실패, 그 외는 성공
                    {
                        if (DataManager.Instance.player.playerData.hp - damage <= 0)
                        {
                            // 실패
                            Console.WriteLine("실패. 체력이 0입니다. 다시 던전 입구로 돌아갑니다. 계속하려면 Enter");
                            // 체력 1로 만들고 던전입구로
                            DataManager.Instance.player.playerData.hp = 1;
                            Console.ReadLine();
                            return;
                        }
                        else
                        {
                            // 2.권장 방어력보다 높다면 성공
                            // 데미지
                            float tempHp = DataManager.Instance.player.playerData.hp;
                            DataManager.Instance.player.playerData.hp -= damage;
                            Console.WriteLine($"체력: {tempHp} -> {DataManager.Instance.player.playerData.hp}");
                            // 기본보상 클리어
                            int tempGold = DataManager.Instance.player.playerData.gold;
                            DataManager.Instance.player.playerData.gold += dungeonInfo.reward;
                            Console.WriteLine($"Gold: {tempGold} G -> {DataManager.Instance.player.playerData.gold} G");
                            // 경험치
                            DataManager.Instance.player.playerData.exp += dungeonInfo.exp;
                            Console.WriteLine($"{dungeonInfo.exp} 경험치를 얻었습니다.");
                            CheckLevelUp();
                            Console.WriteLine();
                            Console.WriteLine($"{dungeonInfo.name} 던전을 공략했습니다!. 던전 입구로 돌아갑니다. 계속하려면 Enter");
                            Console.ReadLine();
                        }
                    }
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
                DataManager.Instance.player.playerData.attack += 0.5f;
                DataManager.Instance.player.playerData.defence += 1.0f;
                DataManager.Instance.player.playerData.maxHp += 10.0f;
                DataManager.Instance.player.playerData.hp += 10.0f;
                DataManager.Instance.player.playerData.exp = remainExp;
                Console.WriteLine($"축하합니다. 레벨이 {DataManager.Instance.player.playerData.level}가 되었습니다");
                Console.WriteLine($"다음 레벨까지 남은 경험치:{10 * DataManager.Instance.player.playerData.level - DataManager.Instance.player.playerData.exp}");
                isLevelUp = DataManager.Instance.player.playerData.exp >= 10 * DataManager.Instance.player.playerData.level ? true : false;
            }
        }
    }
}
