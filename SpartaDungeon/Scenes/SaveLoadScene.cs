using Newtonsoft.Json;
using SpartaDungeon.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpartaDungeon.Scenes
{
    public class SaveLoadScene : BaseScene
    {
        // 저장슬롯 5개를 배열로 선언한다
        public GameData[] dataSlots;

        public SaveLoadScene(string name) : base(name)
        {
            dataSlots = new GameData[5];
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

        // 여기서 저장 로드를 선택한다
        public override void PlayGame()
        {
            Console.Clear();
            Console.WriteLine("[저장 / 불러오기] \n현재까지의 데이터를 저장하거나, 이전의 데이터를 불러올 수 있습니다.\n");
            // 저장슬롯 보여준다
            Console.WriteLine("[저장 슬롯]\n");

            /// json이 저장되는 폴더를 검색하여, 저장파일이 있는지 검사한다 
            CheckSlot();

            for (int i = 0; i < dataSlots.Length; i++)
            {
                Console.Write($"- {i + 1} ");
                if (dataSlots[i] != null)
                {
                    // 해당 슬롯에 이미 저장 데이터가 있다면 저장 데이터를 표시
                    // 플레이어의 이름, 레벨, 무기, 갑옷, 돈 이정도만 표시하자
                    Console.Write($"이름:{dataSlots[i].player.playerData.name} | " + $"레벨:{dataSlots[i].player.playerData.level} | ");
                    if (dataSlots[i].inventory.weapon != null)
                    {
                        Console.Write($"무기:{dataSlots[i].inventory.weapon.itemData.name} | ");
                    }
                    else
                    {
                        Console.Write($"무기:없음 | ");
                    }
                    //
                    if (dataSlots[i].inventory.armor != null)
                    {
                        Console.Write($"갑옷:{dataSlots[i].inventory.armor.itemData.name} | ");
                    }
                    else
                    {
                        Console.Write($"갑옷:없음 | ");
                    }
                    Console.Write($"Gold :{dataSlots[i].player.playerData.gold} G \n");
                }
                else
                {
                    // 없으면 비워놓는다
                    Console.WriteLine("비어있는 저장슬롯");
                }
            }
            Console.WriteLine();
            Console.WriteLine("1~5 중에서 슬롯을 선택하여 저장, 불러오기, 삭제 가능\n0.이전화면으로 돌아가기\n"); // \n4.게임 저장\n5.게임 불러오기\n6.종료
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
            // 슬롯 체크한다
            int fileNum;    /// num은 파일의 인덱스로 배열의인덱스 + 1 이다
            string input = Console.ReadLine();
            if (int.TryParse(input, out fileNum))
            {
                if (fileNum >= 1 && fileNum <= dataSlots.Length)
                {
                    // 슬롯이 비어있었던 경우
                    if (dataSlots[fileNum - 1] == null)
                    {
                        /// 이전 씬의 데이터를 가져와서 저장한다
                        BaseScene prevScene = SceneManager.Instance.GetPrevScene(); // 사실 없어도 된다. DataManager가 씬과 상관없이 존재하기 때문이다
                        if (prevScene != null)
                        {
                            /// 여기가 문제다
                            /// GameData를 멤버로 가진 클래스가 없다. 심지어 DataManager도 GameData를 멤버로 가지고 있지 않다
                            /// 그래서 new로 생성한다.
                            GameData data = new GameData(fileNum - 1, DataManager.Instance.player, DataManager.Instance.inventory); // 실제 배열의 인덱스는 num-1이다
                            /// 하지만 플레이어, 인벤토리가 없는데도 data가 null이 되지 않아서 조건을 바꿨다
                            //if (data != null)
                            if (data.player != null || data.inventory != null)
                            {
                                // 저장
                                dataSlots[fileNum - 1] = data;
                                DataManager.Instance.SaveData(fileNum); /// 파일의 인덱스는 1부터
                                Console.WriteLine($"슬롯 {fileNum}번에 데이터를 저장했습니다.계속하려면 enter.");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("저장할 데이터가 없습니다.계속하려면 enter.");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("이전 씬이 존재하지 않습니다.계속하려면 enter.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"슬롯{fileNum}에는 이미 저장된 데이터가 존재합니다.");
                        ModifySlot(dataSlots[fileNum - 1]); /// dataSlots[0]번은 파일의 1번째이므로, 매개변수로 전달하는 data의 멤버인 fileIndex는 1이 넘어가야한다
                    }
                }
                else if (fileNum == 0)
                {
                    // TownScene에서 왔으면 TownScene으로, GameProcess에서 왔으면 GameProcess로 돌아가야하므로 이전 씬의 이름을 가져온다
                    SceneManager.Instance.SetCurrentScene(SceneManager.Instance.GetPrevScene().GetName());
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                Console.ReadLine();
            }
        }
        public void CheckSlot()
        {
            // json이 저장되어있는 폴더를 검색하여, 해당 저장파일이 존재하면, 파일이름을 슬롯에 넣어야한다
            for (int i = 0; i < dataSlots.Length; i++)
            {
                int slotNumber = i + 1; // 파일 이름에 사용한 번호
                string filePath = DataManager.Instance.GetFilePath(slotNumber); // "savefile_1.json" 등
                if (File.Exists(filePath))
                {
                    try
                    {
                        // 파일에서 JSON 데이터를 읽어들임
                        string jsonData = File.ReadAllText(filePath);
                        // 역직렬화를 위한 설정
                        var settings = new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Include,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        // JSON 데이터를 GameData 객체로 변환
                        GameData loadedData = JsonConvert.DeserializeObject<GameData>(jsonData, settings);
                        dataSlots[i] = loadedData;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"슬롯{slotNumber} 데이터를 불러오는 중 오류 발생: {e.Message}");
                        dataSlots[i] = null;
                    }
                }
                else
                {
                    // 파일이 없으면 해당 슬롯은 비어있는 상태
                    dataSlots[i] = null;
                }
            }
        }
        public void ModifySlot(GameData data)
        {
            // 매개변수로 들어오는 data는 이미 슬롯에 있던 기존의 데이터
            /// dataSlots[0]번은 파일의 1번째이므로, 매개변수로 전달받 data의 멤버인 fileIndex는 1이 되어야 한다. 그런데 0이 넘어왔네?
            bool isValid = false;
            int num;
            while (!isValid)
            {
                Console.Clear();
                Console.WriteLine("[저장 / 불러오기] \n현재까지의 데이터를 저장하거나, 이전의 데이터를 불러올 수 있습니다.\n");
                // 저장슬롯 보여준다
                Console.WriteLine("[저장 슬롯]\n");
                for (int i = 0; i < dataSlots.Length; i++)
                {
                    Console.Write($"- {i + 1} ");
                    if (dataSlots[i] != null)
                    {
                        // 해당 슬롯에 이미 저장 데이터가 있다면 저장 데이터를 표시
                        // 플레이어의 이름, 레벨, 무기, 갑옷, 돈 이정도만 표시하자
                        Console.Write($"이름:{dataSlots[i].player.playerData.name} | " + $"레벨:{dataSlots[i].player.playerData.level} | ");
                        if (dataSlots[i].inventory.weapon != null)
                        {
                            Console.Write($"무기:{dataSlots[i].inventory.weapon.itemData.name} | ");
                        }
                        else
                        {
                            Console.Write($"무기:없음 | ");
                        }
                        //
                        if (dataSlots[i].inventory.armor != null)
                        {
                            Console.Write($"갑옷:{dataSlots[i].inventory.armor.itemData.name} | ");
                        }
                        else
                        {
                            Console.Write($"갑옷:없음 | ");
                        }
                        Console.Write($"Gold :{dataSlots[i].player.playerData.gold} G \n");
                    }
                    else
                    {
                        // 없으면 비워놓는다
                        Console.WriteLine("비어있는 저장슬롯");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("1.불러오기\n2.저장 데이터 덮어쓰기\n3.저장 데이터 삭제\n0.이전화면으로 돌아가기\n"); // \n4.게임 저장\n5.게임 불러오기\n6.종료
                Console.WriteLine("원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out num))
                {
                    switch (num)
                    {
                        case 1:
                            // 불러오기
                            LoadData(data);     // 기존의 데이터 전달
                            isValid = true;
                            break;
                        case 2:
                            OverwriteData(data);    // 기존의 데이터 전달
                            //isValid = true;
                            break;
                        case 3:
                            DeleteData(data);   // 삭제할 데이터 전달
                            isValid = true;
                            break;
                        case 0:
                            /// TownScene에서 왔으면 TownScene으로, GameProcess에서 왔으면 GameProcess로 돌아가야하므로 이전 씬의 이름을 가져온다
                            isValid = true;
                            SceneManager.Instance.SetCurrentScene(SceneManager.Instance.GetPrevScene().GetName());
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.계속하려면 enter.");
                    Console.ReadLine();
                }
            }
        }
        // 불러오기
        public void LoadData(GameData data)
        {
            // 매개변수로 전달된 data는 null이 아니다
            /// 주의: 전달된 data의 인덱스는 파일의 인덱스이므로 1을 더하지 않고 바로 사용한다
            bool isLoaded = DataManager.Instance.LoadData(data.fileIndex);
            if (isLoaded)
            {
                Console.WriteLine($"슬롯 {data.fileIndex}번의 데이터를 불러왔습니다. 계속하려면 Enter를 누르세요.");
                Console.ReadLine();
                // 마을씬으로 바꾼다
                SceneManager.Instance.SetCurrentScene("town"); // 
            }
            else
            {
                Console.WriteLine($"저장된 데이터를 불러오는 데 실패했습니다. 계속하려면 Enter를 누르세요.");
                Console.ReadLine();
            }
        }

        // 덮어쓰기
        public void OverwriteData(GameData data)
        {
            // 새 데이터를 생성 (슬롯 번호는 data.index가 0-based로 저장되어 있으므로, 파일 저장 시에는 +1 해줌)
            GameData newData = new GameData(data.fileIndex, DataManager.Instance.player, DataManager.Instance.inventory);
            // 파일 저장 : DataManager의 SaveData 메서드는 파일 번호(1-based)를 받으므로 data.index + 1을 전달
            /// 하지만 플레이어, 인벤토리가 없는데도 data가 null이 되지 않아서 조건을 바꿨다
            if (newData.player != null || newData.inventory != null)
            {
                DataManager.Instance.SaveData(data.fileIndex);
                // 슬롯 배열 업데이트 (예: dataSlots[data.index] = newData;)
                // 여기서는 ModifySlot의 매개변수 data를 갱신해도 되고, dataSlots 배열의 해당 인덱스를 직접 갱신해도 됨
                dataSlots[data.fileIndex - 1] = newData;    /// fileIndex에서 1을 빼야 배열의 인덱스이다
                Console.WriteLine($"슬롯 {data.fileIndex}번에 새로운 데이터를 저장했습니다. 계속하려면 Enter를 누르세요.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("저장할 데이터가 없습니다.계속하려면 enter.");
                Console.ReadLine();
            }
        }
        // 삭제
        public void DeleteData(GameData data)
        {
            // 매개변수로 들어가는 데이터는 삭제할 데이터
            // 저장된 파일(삭제할 데이터)의 경로를 구함 (data.index가 0부터 시작하므로 파일 번호는 data.index + 1)
            // 1을 눌렀는데 2번이 삭제된다? 1을 더하지 않았는데도
            string filePath = DataManager.Instance.GetFilePath(data.fileIndex);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            // 슬롯 배열에서도 데이터를 제거
            dataSlots[data.fileIndex-1] = null;
            Console.WriteLine($"슬롯 {data.fileIndex}번의 데이터가 삭제되었습니다. 계속하려면 Enter를 누르세요.");
            Console.ReadLine();
        }
    }
}
