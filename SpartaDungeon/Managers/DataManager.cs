using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Managers
{
    public class DataManager : Singleton<DataManager>
    {        
        public Player player;   // 플레이어
        public Inventory inventory; // 플레이어의 아이템

        // 게임 저장, 불러오기
    }
}
