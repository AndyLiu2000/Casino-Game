using System.Collections.Generic;
using UnityEngine;

public class Casino : MonoBehaviour {

    public GameObject LabelWin;
    public GameObject UpBtn;
    public GameObject DownBtn;
    public GameObject H2PBtn;
    public GameObject CasinoBtn;
    public GameObject HowToPlayPanel;
    public GameObject SpinBtn;
    public GameObject AutoSpinBtn;
    UILabel LabelBetNum;
    UILabel LabelChipsNum;
    UILabel LabelCreditNum;

    public static int UserChips = 3000;
    public static int Bet = 60;
    public static int Rank = 1;
    public static int CurrentBet = Bet * Rank;
    public static float WinRate = 3.0f;
    static int WinNum = 0;

    int[] Rotation = new int[5];
    int RotationBegin = 50;
    int SlotNum = 20;
    const int RechargeNum = 3000;

    bool isUpActive;
    bool isDownActive;

    bool IsSpinPressed = false;
    float SpinPressedTime = 0.0f;
    bool IsAutoSpin = false;

    float tempWinNum = 0;
    int tweenTime = 1500;
    bool isTweenText = false;

    Cards[] cards;
    public const int ScrollChildrenNum = 1;
    public static Dictionary<string, GameObject> SlotDic = new Dictionary<string, GameObject>();
    public static GameObject[,] ResultSlots = new GameObject[5, 3];

    // Use this for initialization
    private void Start()
    {
        DataManager.ReadDatas();
        cards = new Cards[DataManager.Cards_Card.Count];
        foreach (Cards_Sheet c in DataManager.Cards_Card)
        {
            int index = int.Parse(c.ID);
            cards[index] = new Cards(int.Parse(DataManager.Cards_Card[index].Value),
                (DirectionType)(int.Parse(DataManager.Cards_Card[index].DirectionType)),
                (CardType)(int.Parse(DataManager.Cards_Card[index].CardType)),
                DataManager.Cards_Card[index].ImgName);
        }

        //读取Chips值
        if (PlayerPrefs.HasKey("IsSaved"))
        {
            //读档
            Debug.Log("Loading Option In Casino....");
            //PlayerPrefs.DeleteAll();
            UserChips = int.Parse(PlayerPrefs.GetString("Chips"));
            Rank = int.Parse(PlayerPrefs.GetString("Rank"));
            

            Debug.Log("Load Option In Casino Complete");
        }

        LabelBetNum = GameObject.Find("LabelBetNum").GetComponent<UILabel>();
        CurrentBet = Bet * Rank;
        LabelBetNum.text = CurrentBet.ToString();

        LabelChipsNum = GameObject.Find("LabelChipsNum").GetComponent<UILabel>();
        LabelChipsNum.text = UserChips.ToString();

        LabelCreditNum = GameObject.Find("LabelCreditNum").GetComponent<UILabel>();
        LabelCreditNum.text = Rank.ToString();

        LabelWin.GetComponent<UILabel>().text = "0";
        LabelWin.SetActive(false);

        isUpActive = true;
        isDownActive = true;

        if (Rank == 1)
        {
            isDownActive = false;
            isUpActive = true;
            DownBtn.SetActive(isDownActive);
            UpBtn.SetActive(isUpActive);
        }
        else if(Rank == 5)
        {
            isDownActive = true;
            isUpActive = false;
            DownBtn.SetActive(isDownActive);
            UpBtn.SetActive(isUpActive);
        }

        HowToPlayPanel.SetActive(false);

        for (int i = 1; i <= Rotation.Length; i++)
        {
            GameObject scrollSLot = GameObject.Find("ScrollSlot" + i);
            GameObject uiwrapcontent = GameObject.Find("UIWrapContent" + i);
            for (int j = 1; j <= SlotNum; j++)
            {
                GameObject slot = Resources.Load("Slot") as GameObject;
                slot.name = "Slot" + i + j;
                Vector2 pos = new Vector2(0, -252 * (j - 1));
                int randomCardIndex = Random.Range(0, cards.Length);
                slot.GetComponent<Slot>().Card = cards[randomCardIndex];
                slot.GetComponent<UISprite>().spriteName = slot.GetComponent<Slot>().Card.ImgName;
                slot.GetComponent<Slot>().Color = slot.GetComponent<UISprite>().color;

                GameObject s = NGUITools.AddChild(uiwrapcontent, slot);

                s.transform.localPosition = pos;
                SlotDic.Add(slot.name, s);
            }
            //重排位置
            uiwrapcontent.GetComponent<UICenterOnChild>().Recenter();
        }

        GameObject scrollSLot5 = GameObject.Find("ScrollSlot" + 5);
        scrollSLot5.GetComponent<UIScrollView>().onStoppedMoving += delegate () {
            GameObject.Find(AudioManager.SCROLL).GetComponent<AudioSource>().Stop();
            GameObject.Find(AudioManager.SCROLLEND).GetComponent<AudioSource>().Play();
            //延迟一秒再结算，这一秒等待Scrollview对齐；如果不等待，结果会出错
            Invoke("CalResult", 1.0f);
        };

        SpinBtn.SetActive(true);
        AutoSpinBtn.SetActive(false);

        UIEventListener.Get(AutoSpinBtn).onClick = delegate (GameObject btn) {
            GameObject.Find(AudioManager.SPIN).GetComponent<AudioSource>().Play();
            IsAutoSpin = false;
            SpinBtn.SetActive(true);
            AutoSpinBtn.SetActive(false);
        };

        UIEventListener.Get(SpinBtn).onClick = ClickSpinBtn;

        UIEventListener.Get(SpinBtn).onPress = delegate (GameObject go, bool isPressed)
        {
            IsSpinPressed = isPressed;
        };

        UIEventListener.Get(GameObject.Find("RechargeBtn")).onClick = delegate (GameObject btn)
        {
            Debug.Log("RechargeBtn");
            UserChips += RechargeNum;
            LabelChipsNum.text = UserChips.ToString();
            if (UserChips > 0) SpinBtn.GetComponent<UIButton>().isEnabled = true;
        };

        UIEventListener.Get(UpBtn).onClick = delegate (GameObject btn)
        {
            Debug.Log("UpBtn");
            GameObject.Find(AudioManager.SPIN).GetComponent<AudioSource>().Play();
            if(Rank < 5)
            {
                Rank++;
                CurrentBet = Rank * Bet;
                LabelBetNum.text = CurrentBet.ToString();
                LabelCreditNum.text = Rank.ToString();
                if(Rank == 5)
                {
                    isUpActive = false;
                    UpBtn.SetActive(isUpActive);
                }
                else
                {
                    isDownActive = true;
                    DownBtn.SetActive(isDownActive);
                }
                PlayerPrefs.SetString("Rank", Rank.ToString());
            }
        };

        UIEventListener.Get(DownBtn).onClick = delegate (GameObject btn)
        {
            Debug.Log("DownBtn");
            GameObject.Find(AudioManager.SPIN).GetComponent<AudioSource>().Play();
            if (Rank >1)
            {
                Rank--;
                CurrentBet = Rank * Bet;
                LabelBetNum.text = CurrentBet.ToString();
                LabelCreditNum.text = Rank.ToString();
                if (Rank == 1)
                {
                    isDownActive = false;
                    DownBtn.SetActive(isDownActive);
                }
                else
                {
                    isUpActive = true;
                    UpBtn.SetActive(isUpActive);
                }
                PlayerPrefs.SetString("Rank", Rank.ToString());
            }
        };

        UIEventListener.Get(H2PBtn).onClick = delegate (GameObject btn)
        {
            Debug.Log("H2PBtn");
            HowToPlayPanel.SetActive(true);
        };

        UIEventListener.Get(CasinoBtn).onClick = delegate (GameObject btn)
        {
            Debug.Log("CasinoBtn");
            HowToPlayPanel.SetActive(false);
        };
    }

    void ClickSpinBtn(GameObject go)
    {
        Debug.Log("SpinBtn");
        UpBtn.SetActive(false);
        DownBtn.SetActive(false);
        GameObject.Find(AudioManager.SPIN).GetComponent<AudioSource>().Play();
        SpinBtn.GetComponent<UIButton>().isEnabled = false;
        LabelWin.SetActive(false);
        for (int i = 1; i <= Rotation.Length; i++)
        {
            GameObject scrollSLot = GameObject.Find("ScrollSlot" + i);
            GameObject uiwrapcontent = GameObject.Find("UIWrapContent" + i);
            for (int j = 1; j <= SlotNum; j++)
            {
                int randomCardIndex = Random.Range(0, cards.Length);
                SlotDic["Slot" + i + j].GetComponent<Slot>().Card = cards[Formula.Spin(i)];
                SlotDic["Slot" + i + j].GetComponent<UISprite>().spriteName = SlotDic["Slot" + i + j].GetComponent<Slot>().Card.ImgName;
                SlotDic["Slot" + i + j].GetComponent<UISprite>().color = SlotDic["Slot" + i + j].GetComponent<Slot>().Color;
                SlotDic["Slot" + i + j].transform.localRotation = new Quaternion(0, 0, 0, 0);
                switch (SlotDic["Slot" + i + j].GetComponent<Slot>().Card.DT)
                {
                    case DirectionType.L:
                        SlotDic["Slot" + i + j].transform.Rotate(0, 0, -45);
                        break;
                    case DirectionType.R:
                        SlotDic["Slot" + i + j].transform.Rotate(0, 0, 45);
                        break;
                    default:
                        break;
                }
            }
            //这里修改过UIScrollView里的代码，使scrollview快速停下来，触发onStoppedMoving委托
            //这里还改过UICenterOnChild，使重新居中的操作在onStoppedMoving时触发一下，保证最后总是转动到居中的位置来保证对齐
            scrollSLot.GetComponent<UIScrollView>().Scroll(RotationBegin * i);
        }
    }

    void CalResult()
    {
        Debug.Log("CalResult");
        for (int i = 1; i <= Rotation.Length; i++)
        {
            for (int l = 1; l <= SlotNum; l++)
            {
                if (SlotDic["Slot" + i + l].activeSelf)
                {
                    if (FindNextActive(i, l, 4))
                    {
                        ResultSlots[i - 1, 0] = FindNext(i, l);
                        ResultSlots[i - 1, 1] = FindNext(i, l + 1);
                        ResultSlots[i - 1, 2] = FindNext(i, l + 2);
                        break;
                    }
                }
            }
        }

        //计算分数
        //三横排：1，2，3号线赢
        //WinNum += Line1_2_3Win();
        WinNum += Formula.Win1_2_3();

        //右斜排
        //4号线赢
        WinNum += Formula.Win4_8(0, 3, 1, 4);
        //8号线赢
        WinNum += Formula.Win4_8(1, 0, 2, 1);
        //5号线赢
        WinNum += Formula.Win5_6_7(0, 2, 1, 3, 2, 4);
        //6号线赢
        WinNum += Formula.Win5_6_7(0, 1, 1, 2, 2, 3);
        //7号线赢
        WinNum += Formula.Win5_6_7(0, 0, 1, 1, 2, 2);

        //左斜排
        //9号线赢
        WinNum += Formula.Win9_13(1, 4, 2, 3);
        //13号线赢
        WinNum += Formula.Win9_13(0, 1, 1, 0);
        //12号线赢
        WinNum += Formula.Win10_11_12(0, 2, 1, 1, 2, 0);
        //11号线赢
        WinNum += Formula.Win10_11_12(0, 3, 1, 2, 2, 1);
        //10号线赢
        WinNum += Formula.Win10_11_12(0, 4, 1, 3, 2, 2);
        //下折线赢
        //14号线赢
        WinNum += Formula.Win14(0, 0, 1, 1, 2, 2, 1, 3, 0, 4);
        //上折线赢
        //15号线赢
        WinNum += Formula.Win15(2, 0, 1, 1, 0, 2, 1, 3, 2, 4);

        //SkyWheel赢
        WinNum += Formula.Win_SkyWheel();
        //汇总结算
        if(WinNum > 0)
        {
            LabelWin.SetActive(true);
            isTweenText = true;
            Invoke("ExecuteResult", tweenTime * 1.0f / 1000 + 1.0f);
        }
        else ExecuteResult();
    }

    void ExecuteResult()
    {
        if(WinNum>0) GameObject.Find(AudioManager.WINSOUND).GetComponent<AudioSource>().Play();
        isTweenText = false;
        LabelWin.GetComponent<UILabel>().text = WinNum.ToString();
        UserChips = UserChips - CurrentBet + WinNum;
        //存档
        PlayerPrefs.SetString("Chips", UserChips.ToString());
        LabelChipsNum.text = UserChips.ToString();

        SpinBtn.GetComponent<UIButton>().isEnabled = true;

        if (UserChips <= 0)
        {
            SpinBtn.GetComponent<UIButton>().isEnabled = false;
        }
        else
        {
            if (IsAutoSpin) ClickSpinBtn(null);
        }
        tempWinNum = 0;
        WinNum = 0;
        UpBtn.SetActive(isUpActive);
        DownBtn.SetActive(isDownActive);
    }

    bool FindNextActive(int rotation,int index, int checkStep)
    {    
        if(checkStep == 0) return true;

        if (index >= 1 && index < SlotNum)
        {
            if (SlotDic["Slot" + rotation + (index + 1)].activeSelf)
            {
                return FindNextActive(rotation, index + 1,checkStep - 1);
            }
            else return false;  
        }
        if (index >= SlotNum)
        {
            if (SlotDic["Slot" + rotation + (index - SlotNum + 1)].activeSelf)
            {
                return FindNextActive(rotation, (index - SlotNum + 1), checkStep - 1);
            }
            else return false;
        }
        return false;
    }

    GameObject FindNext(int rotation,int index)
    {
        if (index >= 1 && index < SlotNum) return SlotDic["Slot" + rotation + (index + 1)];
        if(index >= SlotNum) return SlotDic["Slot" + rotation + (index - SlotNum + 1)];
        return null;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        if (IsSpinPressed)
        {
            SpinPressedTime += Time.fixedDeltaTime;
            if (SpinPressedTime >= 1.0f)
            {
                //切换为自动摇奖
                IsAutoSpin = true;
                SpinBtn.SetActive(false);
                AutoSpinBtn.SetActive(true);
                ClickSpinBtn(null);
                SpinPressedTime = 0.0f;
                IsSpinPressed = false;
            }
        }

        if (isTweenText)
        {
            if (tempWinNum < WinNum)
            {
                tempWinNum += WinNum * Time.fixedDeltaTime / (tweenTime * 1.0f/1000);
                LabelWin.GetComponent<UILabel>().text = ((int)tempWinNum).ToString();
            }
        }
	}
}
