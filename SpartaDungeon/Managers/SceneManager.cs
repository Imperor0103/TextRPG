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
        BaseScene prevScene;
        BaseScene currentScene;
        //List<BaseScene> sceneList;
        public Dictionary<string, BaseScene> sceneDictionary;

        // scene의 이름을 List<string>에 저장해서 유니티의 BuildIndex처럼 사용할 수 있겠다
        // 따로 선언하지 않고 GetCurrentSceneIndex로 Dictionary에서 인덱스를 가져올 수 있게 만들었다

        public void PrepareScene()
        {
            if (sceneDictionary == null)
            {
                sceneDictionary = new Dictionary<string, BaseScene>();
            }
            BaseScene entryScene = new EntryScene("entry");
            BaseScene townScene = new TownScene("town");
            BaseScene storeScene = new StoreScene("store");
            BaseScene dungeonScene = new DungeonScene("dungeon");
            BaseScene saveLoadScene = new SaveLoadScene("saveLoad");

            sceneDictionary.Add("entry", entryScene);
            sceneDictionary.Add("town", townScene);
            sceneDictionary.Add("store",storeScene);
            sceneDictionary.Add("dungeon",dungeonScene);
            sceneDictionary.Add("saveLoad", saveLoadScene);
            // 위의 순서대로 빌드인덱스를 정해서 List<string>에 저장해보자

            //sceneList.Add(townScene);
            //sceneList.Add(storeScene);
            //sceneList.Add(dungeonScene);
            // 처음에는 entry씬
            currentScene = entryScene;
        }
        public void Start()
        {
            currentScene.Start();
        }
        public void Update()
        {
            currentScene.Update();
        }
        public void SetCurrentScene(string key)
        {
            // 씬을 로드하고, 현재 씬을 바꾼다
            prevScene = currentScene;   /// saveLoad 씬에서 0을 눌렀을때 이전에 왔던 화면으로 돌아가야한다
            // 사실 현재 콘솔프로젝트에서는 씬을 로드하는 것이 필요하지 않다
            // 앞으로 사용할 경우를 위해 연습하는 것이다
            currentScene.UnloadScene(); // 현재 씬을 언로드(하는 척만 한다)
            currentScene = sceneDictionary[key];    // 현재씬을 교체후
            currentScene.LoadScene();       // 새로운 씬을 로드(하는 척만 한다)
            // 로드한 다음에는 해당 씬을 Start해야한다
            Start();    // Start에서 알아서 currentScene의 Start를 호출
        }
        public BaseScene GetCurrentScene()
        {
            return currentScene;
        }
        public BaseScene GetPrevScene()
        {
            return prevScene;
        }

        // 유니티의 BuildIndex 가져오는 기능
        public int GetCurrentSceneIndex()
        {
            int index = 0;
            foreach (var pair in sceneDictionary)
            {
                if (pair.Value == currentScene)
                {
                    return index;
                }
                index++;
            }
            return -1;  // 못 찾으면 -1 리턴
        }
        public int GetCurrentSceneindex2()
        {
            return sceneDictionary.Keys.ToList().IndexOf(currentScene.GetName()); 
        }

    }
}
