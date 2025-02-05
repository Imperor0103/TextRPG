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
        //public int expToNextLevel;  // 다음 레벨까지 필요한 경험치는 직접 만들지 말고, level을 이용하여 공식으로 처리한다
        // 다음 레벨까지 필요한 경험치 = level * 10
        public int exp;        // 경험치
    }

    public class Player
    {
        public PlayerData playerData;
        public Player()
        {
        }
        // 플레이어 생성
        public void SetPlayerData(int lv, eClassType type, float atk, float def, float maxH, float h, int gold, int e = 0)
        {
            playerData.level = lv;
            playerData.charaClass = type;
            playerData.attack = atk;
            playerData.defence = def;
            playerData.maxHp = maxH;
            playerData.hp = h;
            playerData.gold = gold;
            playerData.exp = e;     // e는 입력하지 않는 경우 기본값 0으로 세팅
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
        public float PlayerAttack()
        {
            if (DataManager.Instance.inventory.Weapon != null)
            {
                return playerData.attack + DataManager.Instance.inventory.Weapon.itemData.attack;
            }
            else
            {
                return playerData.attack;
            }
        }
        public float PlayerDefence()
        {
            if (DataManager.Instance.inventory.Armor != null)
            {
                return playerData.defence + DataManager.Instance.inventory.Armor.itemData.defence;
            }
            else
            {
                return playerData.defence;
            }
        }
    }
}
