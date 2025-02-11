using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Managers
{
    // player와 inventory를 하나의 파일로 저장하기 위해 GameData 클래스로 래핑한다
    public class GameData
    {
        public int fileIndex;   /// 저장시점에 저장슬롯의 인덱스(배열의 인덱스 + 1)를 대입한다 
        public Player player;   // 참조 타입의 멤버 변수
        public Inventory inventory; // 참조 타입의 멤버 변수
        public GameData()
        {
            int index = 0;
            /// player, inventory를 생성하지 않으면 둘 다 null이 되어 LoadData 메서드에서 대입을 못한다
            player = new Player();
            inventory = new Inventory();
        }
        public GameData(int idx, Player p, Inventory i)
        {
            fileIndex = idx;
            player = p;
            inventory = i;
        }
    }

    public class DataManager : Singleton<DataManager>
    {
        public Player player;   // 플레이어
        public Inventory inventory; // 플레이어의 아이템
        // 게임 저장, 불러오기
        // Newtonsoft.Json을 이용하여 저장한다

        // 파일경로
        /// <summary>
        /// 주의: fileIdx는 파일에 붙은 숫자를 뜻하며, SaveLoadScene이 가지고 있는 배열의 인덱스보다 1 크다
        /// </summary>
        /// <param name="fileIdx"></param>
        /// <returns></returns>
        public string GetFilePath(int fileIdx)
        {
            // 세이브파일 생성위치 수정
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string parentDirectory = Directory.GetParent(exeDirectory).Parent.Parent.Parent.FullName;   // 3단계 상위 폴더(프로젝트폴더)로 이동
            string saveDirectory = Path.Combine(parentDirectory, "SaveFiles");
            Directory.CreateDirectory(saveDirectory);
            return Path.Combine(saveDirectory,$"savefile_{fileIdx}.json"); // 실행파일이 있는 곳에 해당 이름으로 저장된다
        }

        public bool IsSaveFile(int fileIdx)
        {
            return File.Exists(GetFilePath(fileIdx));
        }
        public void SaveData(int fileIdx)
        {
            // 인덱스가 넘어온다

            GameData tmpData = new GameData(fileIdx, player, inventory);
            // null이어도 저장이 되어야한다!

            string json = JsonConvert.SerializeObject(tmpData);
            File.WriteAllText(GetFilePath(tmpData.fileIndex), json);
        }
        public bool LoadData(int fileIdx)
        {
            /// 주의: 기본생성자로 할당할 경우 player, inventory가 null이라서 읽은 파일의 데이터를 대입할 수 없다
            /// 아니었다. 기본생성자에 아무것도 없어도 
            /// gameData = JsonConvert.DeserializeObject<GameData>(jsonData, settings); 
            /// 를 호출하니까 player, inventory에 자동으로 객체를 생성해서 대입했다
            /// 
            /// DataManager의 멤버변수 player, inventory가 null이라서 그런 거였다
            player = new Player();  // DataManager의 멤버
            inventory = new Inventory();
            GameData gameData = new GameData(); // 메모리 할당


            string path = GetFilePath(fileIdx);
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    MissingMemberHandling = MissingMemberHandling.Ignore    // JSON 데이터에 C# 객체에 정의되지 않은 속성이 있을 때, 이를 무시하고 계속 진행할 수 있습니다 
                };
                gameData = JsonConvert.DeserializeObject<GameData>(jsonData, settings);
                // 플레이어
                player.playerData.name = gameData.player.playerData.name;
                player.playerData.level = gameData.player.playerData.level;
                player.playerData.charaClass = gameData.player.playerData.charaClass;
                player.playerData.attack = gameData.player.playerData.attack;
                player.playerData.defence = gameData.player.playerData.defence;
                player.playerData.maxHp = gameData.player.playerData.maxHp;
                player.playerData.hp = gameData.player.playerData.hp;
                player.playerData.gold = gameData.player.playerData.gold;
                player.playerData.exp = gameData.player.playerData.exp;
                // 인벤토리
                foreach (var data in gameData.inventory.ownedItemSet)
                {
                    inventory.ownedItemSet.Add(data);
                }
                foreach (var data in gameData.inventory.armedItemList)
                {
                    if (gameData.inventory.armedItemList.Contains(data))
                    {
                        inventory.armedItemList.Add(data);
                    }
                }
                // nullable이므로 아래의 내용 없이 들어가는지 확인해봐야한다
                inventory.weapon = gameData.inventory.weapon;
                inventory.armor = gameData.inventory.armor;
                //if (gameData.inventory.weapon != null)
                //{
                //    inventory.weapon = gameData.inventory.weapon;
                //}
                //if (gameData.inventory.armor != null)
                //{
                //    inventory.armor = gameData.inventory.armor;
                //}
                return true;
            }
            return false;
        }

    }
}
