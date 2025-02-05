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

        static public bool isPlaying;

        // 유니티의 Awake 같은 기능
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
                // 씬을 준비한다(초기 currentScene은 시작화면)
                sceneManager.PrepareScene();
            }
            isPlaying = true;   // 값을 바꾸지만 않는다면, 무한루프 돈다
        }
        public void Start()
        {
            sceneManager.Start();
        }
 
        public void Loop()
        {
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
