using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    //存储所有声音物体名
    public const string MainBGM = "MainBGM";
    public const string SCROLL = "Scroll";
    public const string SCROLLEND = "ScrollEnd";
    public const string SPIN = "Spin";
    public const string WINSOUND = "WinSound";

    //记录当前播放的声音物体名
    private static string currrentBG = "";

    //声明音量字段
    private static float bgVolume = 0.3f;
    private static float voiceVolume = 0.8f;
    private static bool isSoundOn;

    public static float BgVolume
    {
        get
        {
            return bgVolume;
        }
        set
        {
            bgVolume = value;
        }
    }

    public static float VoiceVolume
    {
        get
        {
            return voiceVolume;
        }
        set
        {
            voiceVolume = value;
        }
    }

    public static bool IsSoundOn
    {
        get
        {
            return isSoundOn;
        }
        set
        {
            isSoundOn = value;
        }
    }

    //建立声音数据库
    public static Dictionary<string, AudioSource> AudioSources = new Dictionary<string, AudioSource>();

    void Awake () {
        Debug.Log("AudioManager.start");
        AudioSources.Add(MainBGM,GameObject.Find(MainBGM).GetComponent<AudioSource>());
        AudioSources.Add(SCROLL, GameObject.Find(SCROLL).GetComponent<AudioSource>());
        AudioSources.Add(SCROLLEND, GameObject.Find(SCROLLEND).GetComponent<AudioSource>());
        AudioSources.Add(SPIN, GameObject.Find(SPIN).GetComponent<AudioSource>());
        AudioSources.Add(WINSOUND, GameObject.Find(WINSOUND).GetComponent<AudioSource>());

        //设定初始音量，要做读设置的处理
        bgVolume = 0.3f;
        voiceVolume = 0.8f;
        isSoundOn = true;
        //playMusicByName(MainBGM);
        //GameObject.Find(MainBGM).GetComponent<AudioSource>().Play();
    }

    void Start()
    {
        GameObject.Find(MainBGM).GetComponent<AudioSource>().volume = bgVolume;
        GameObject.Find(MainBGM).GetComponent<AudioSource>().Play();
        //playMusicByName(MainBGM);
    }

    public static void playMusicByName(string musicName)
    {
        /*
        foreach (string bgName in AudioSources.Keys)
        {
            if (bgName == currrentBG)
            {
                AudioSources[bgName].Stop();
            }
        }*/

        AudioSources[currrentBG].Stop();

        AudioSources[musicName].volume = bgVolume;
        AudioSources[musicName].Play();
        currrentBG = musicName;
    }

    public static void PlayVoiceByName(string voiceName)
    {
        foreach (string bgName in AudioSources.Keys)
        {
            if(bgName != MainBGM)
                AudioSources[bgName].Stop();
        }
        GameObject.Find(voiceName).GetComponent<AudioSource>().volume = voiceVolume;
        GameObject.Find(voiceName).GetComponent<AudioSource>().Play();
    }

    public static void ChangeBGVolumeTo(float volume)
    {
        bgVolume = volume;
        foreach (string bgName in AudioSources.Keys)
        {
            AudioSources[bgName].volume = bgVolume;
        }
    }

    public static void ChangeMEToggle(bool soundOn)
    {
        isSoundOn = soundOn;
        if (isSoundOn)
        {
            play();
        }
        else
        {
            Mute();
        }
    }

    public static void Mute()
    {
        AudioListener audioListener = GameObject.Find("Camera").GetComponent<AudioListener>();
        audioListener.enabled = false;
    }

    public static void play()
    {
        AudioListener audioListener = GameObject.Find("Camera").GetComponent<AudioListener>();
        audioListener.enabled = true;
        
    }

}
