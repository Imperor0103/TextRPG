using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum eClassType
    {
        NONE = -1,
        WARRIOR = 1 << 0,   // 0001
        MAGE = 1 << 1,      // 0010
        ARCHER = 1 << 2,    // 0100
        // ALL은 할당 후, and연산 처리 -> 결과가 1개 나오면 그 타입을 출력함
        ALL = WARRIOR | MAGE | ARCHER  // 0111
    }

    public struct PlayerData
    {
        public string name;    // 이름
        public int level;      // 레벨
        public eClassType charaClass;   // 직업
        public float attack;   // 공격력
        public float defence;  // 방어력
        public float maxHp;    // 최대 체력
        public float hp;       // 체력
        public int gold;       // 소지금
    }

    public class Player
    {
        public PlayerData playerData;
        public Player()
        {
        }
        // 플레이어 생성
        public void SetPlayerData(int lv, eClassType type, float atk, float def, float maxH, float h, int gold)
        {
            playerData.level = lv;
            playerData.charaClass = type;
            playerData.attack = atk;
            playerData.defence = def;
            playerData.maxHp = maxH;
            playerData.hp = h;
            playerData.gold = gold;
            //
        }
        public void SetPlayerName(string name)
        {
            playerData.name = name;
        }
        // 플레이어 불러오기
        public void SetPlayerData(string name, int lv, eClassType type, float atk, float def, float h, int gold)
        {
            playerData.name = name;
            playerData.level = lv;
            playerData.charaClass = type;
            playerData.attack = atk;
            playerData.defence = def;
            playerData.hp = h;
            playerData.gold = gold;
        }
        // 공격, 방어시에는 아이템 능력치까지 합산한다
        public float Attack()
        {
            return playerData.attack + DataManager.Instance.inventory.Weapon.itemData.attack;
        }
        public float Defence()
        {
            return playerData.defence + DataManager.Instance.inventory.Weapon.itemData.defence;
        }
    }
}
