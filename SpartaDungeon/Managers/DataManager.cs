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
        public int index;   // 저장시점에 저장슬롯의 인덱스를 대입한다
        public Player player;
        public Inventory inventory;
        public GameData() { }
        public GameData(int idx, Player p, Inventory i)
        {
            index = idx;
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
        public string GetFilePath(int idx)
        {
            return $"savefile_{idx}.json"; // 실행파일이 있는 곳에 해당 이름으로 저장된다
        }

        public bool IsSaveFile(int idx)
        {
            return File.Exists(GetFilePath(idx));
        }
        public void SaveData(int idx)
        {
            // 인덱스가 넘어온다

            GameData tmpData = new GameData(idx, player, inventory);
            // null이어도 저장이 되어야한다!

            string json = JsonConvert.SerializeObject(tmpData);
            File.WriteAllText(GetFilePath(tmpData.index), json);
        }
        public bool LoadData(int idx)
        {
            GameData gameData = new GameData(); // 메모리 할당을 먼저 한다

            string path = GetFilePath(idx);
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
