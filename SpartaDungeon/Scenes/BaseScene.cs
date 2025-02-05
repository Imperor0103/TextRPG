using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Scenes
{    
    // 부모의 인스턴스 생성을 막기 위해 추상클래스로 만들었다
    public abstract class BaseScene
    {
        // 씬의 이름
        string sceneName;
        public BaseScene(string name)
        {
            sceneName = name;
        }
        public string GetName()
        {
            return sceneName;
        }
        public abstract void Start();
        public abstract void LoadScene();   // 추상클래스에서는 virtual을 사용 불가
        public abstract void UnloadScene();
        public abstract void Update();
        public abstract void PlayGame();
    }
}
