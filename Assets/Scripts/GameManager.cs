using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //存储所有UI物体名------下一版本将战斗界面独立到另一个Scene，DontDestroyOnLoad()
    public const string CASINO = "Casino";

    //存档地址
    public static string FilePathName;

    //建立目录Scenes数据库
    public static Dictionary<string, GameObject> UIS = new Dictionary<string, GameObject>();

    void Awake () {
        Debug.Log("GameManager.start");

        Debug.Log("SystemLanguage = " + Application.systemLanguage.ToString());

        //读取数据表
        //DataManager.ReadDatas();

        //读设置
        if (PlayerPrefs.HasKey("IsSaved"))
        {
            //读档
            Debug.Log("Loading Option....");
            //需要清档测试的时候就取消注释运行一下
            //PlayerPrefs.DeleteAll();
            AudioManager.BgVolume = PlayerPrefs.GetFloat("MusicVolume");
            AudioManager.IsSoundOn = bool.Parse(PlayerPrefs.GetString("IsSoundOn"));

            Debug.Log("Load Option Complete");
        }
        else
        {
            //存储设置
            Debug.Log("Saving Option....");
            PlayerPrefs.SetString("IsSaved", "Yes");
            PlayerPrefs.SetFloat("MusicVolume", AudioManager.BgVolume);
            PlayerPrefs.SetString("IsSoundOn", AudioManager.IsSoundOn.ToString());
            //PlayerPrefs.SetString("Chips", Casino.UserChips.ToString());
            //PlayerPrefs.SetString("Rank", Casino.Rank.ToString());
            Debug.Log("Save Option Complete");
        }

        //用户数据读档、存档

        //定义存档路径
        string dirpath = Application.persistentDataPath + "/Save";
        //创建存档文件夹
        IOHelper.CreateDirectory(dirpath);
        //定义存档文件路径
        string filename = dirpath + "/Casino_GameData.sav";

        FilePathName = filename;

        //如果文件存在，读档
        if (IOHelper.IsFileExists(FilePathName))
        {
            LoadData();
        }
        //如果文件不存在，新建档案
        else
        {
            //新建数据，并保存数据
            SaveData();
        }

        //把界面都包进字典
        UIS.Add(CASINO, GameObject.Find(CASINO));

        //初始化第一个界面
        UIS[CASINO].SetActive(true);

        GameObject Exit = GameObject.Find("Exit");
        UIEventListener.Get(Exit).onClick = delegate (GameObject btn)
        {
            Debug.Log("Exit_Click");
            Application.Quit();
        };
    }

    private void Start()
    {
    }
	
    public static void SaveData()
    {
        //保存数据
        Debug.Log("Saving Data....");

        Debug.Log("Save Data Complete");
    }

    public static void LoadData()
    {
        //读取数据
        Debug.Log("Loading Data....");

        //将存档反序列化到一个临时库中，再转换成正常值

        Debug.Log("Load Data Complete");
    }
}
