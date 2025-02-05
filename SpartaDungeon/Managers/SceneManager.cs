using SpartaDungeon.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Managers
{
    public class SceneManager : Singleton<SceneManager>
    {
        // 모든 씬을 관리한다
        // 마을, 던전, 상점
        BaseScene currentScene;
        //List<BaseScene> sceneList;
        Dictionary<string, BaseScene> sceneDictionary;

        public void PrepareScene()
        {
            if (sceneDictionary == null)
            {
                sceneDictionary = new Dictionary<string, BaseScene>();
            }
            BaseScene townScene = new TownScene("town");
            BaseScene storeScene = new StoreScene("store");
            BaseScene dungeonScene = new DungeonScene("dungeon");

            sceneDictionary.Add("town", townScene);
            sceneDictionary.Add("store",storeScene);
            sceneDictionary.Add("dungeon",dungeonScene);

            //sceneList.Add(townScene);
            //sceneList.Add(storeScene);
            //sceneList.Add(dungeonScene);
            // 처음에는 마을
            currentScene = townScene;
        }
        public void Update()
        {
            currentScene.Update();
        }
        public void SetCurrentScene(string key)
        {
            // 씬을 로드하고, 현재 씬을 바꾼다
            // 사실 현재 콘솔프로젝트에서는 씬을 로드하는 것이 필요하지 않다
            // 앞으로 사용할 경우를 위해 연습하는 것이다
            currentScene.UnloadScene(); // 현재 씬을 언로드(하는 척만 한다)
            currentScene = sceneDictionary[key];    // 현재씬을 교체후
            currentScene.LoadScene();       // 새로운 씬을 로드(하는 척만 한다)
        }
        public BaseScene GetCurrentScene()
        {
            return currentScene;
        }
    }
}
