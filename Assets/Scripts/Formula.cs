using System.Collections.Generic;
using UnityEngine;
using System;

public class Formula {
    //出牌概率
    public static int Spin(int rotationID)
    {
        //weight objects list
        List<Cards_Sheet> weightLoot = new List<Cards_Sheet>();
        Type t = typeof(Cards_Sheet);

        foreach (Cards_Sheet cs in DataManager.Cards_Card)
        {
            //objects with the same rotationid will be in the same weightloot
            switch (rotationID)
            {
                case 1:
                    //will not count into loot if equals to 0
                    if (cs.Weight_1 != "0") weightLoot.Add(cs);
                    break;
                case 2:
                    //will not count into loot if equals to 0
                    if (cs.Weight_2 != "0") weightLoot.Add(cs);
                    break;
                case 3:
                    //will not count into loot if equals to 0
                    if (cs.Weight_3 != "0") weightLoot.Add(cs);
                    break;
                case 4:
                    //will not count into loot if equals to 0
                    if (cs.Weight_4 != "0") weightLoot.Add(cs);
                    break;
                case 5:
                    //will not count into loot if equals to 0
                    if (cs.Weight_5 != "0") weightLoot.Add(cs);
                    break;
                default:
                    break;
            }
        }

        int finalLoot = -1;
        //count weight only if number of objects is greater than 0
        if (weightLoot.Count > 0)
        {
            //sum weight
            int weightSum = 0;
            foreach (Cards_Sheet w in weightLoot)
            {
                switch (rotationID)
                {
                    case 1:
                        weightSum += int.Parse(w.Weight_1);
                        break;
                    case 2:
                        weightSum += int.Parse(w.Weight_2);
                        break;
                    case 3:
                        weightSum += int.Parse(w.Weight_3);
                        break;
                    case 4:
                        weightSum += int.Parse(w.Weight_4);
                        break;
                    case 5:
                        weightSum += int.Parse(w.Weight_5);
                        break;
                    default:
                        break;
                }
            }

            float random = UnityEngine.Random.Range(0.0f, 1.0f);

            for (int i = 0; i < weightLoot.Count; i++)
            {
                float curWeightSum = 0;
                for (int j = 0; j <= i; j++)
                {
                    switch (rotationID)
                    {
                        case 1:
                            curWeightSum += float.Parse(weightLoot[j].Weight_1) / (float)weightSum;
                            break;
                        case 2:
                            curWeightSum += float.Parse(weightLoot[j].Weight_2) / (float)weightSum;
                            break;
                        case 3:
                            curWeightSum += float.Parse(weightLoot[j].Weight_3) / (float)weightSum;
                            break;
                        case 4:
                            curWeightSum += float.Parse(weightLoot[j].Weight_4) / (float)weightSum;
                            break;
                        case 5:
                            curWeightSum += float.Parse(weightLoot[j].Weight_5) / (float)weightSum;
                            break;
                        default:
                            break;
                    }
                    //random number drops in the area of curWeightSum
                    if (random < curWeightSum)
                    {
                        finalLoot = int.Parse(weightLoot[j].ID);
                        goto End;
                    }
                }
            }
        }

        End:
        return finalLoot;
    }

    //Line1,2,3 win
    public static int Win1_2_3()
    {
        int WinNum = 0;
        for (int i = 0; i < 3; i++)
        {
            int SameNum = 1;
            for (int j = 0; j < 4; j++)
            {
                if (Casino.ResultSlots[j, i].GetComponent<Slot>().Card.CT == Casino.ResultSlots[j + 1, i].GetComponent<Slot>().Card.CT)
                {
                    if (Casino.ResultSlots[j, i].GetComponent<Slot>().Card.DT == Casino.ResultSlots[j + 1, i].GetComponent<Slot>().Card.DT
                        && Casino.ResultSlots[j, i].GetComponent<Slot>().Card.DT == DirectionType.H)
                    {
                        if (Casino.ResultSlots[j, i].GetComponent<Slot>().Card.Value == Casino.ResultSlots[j + 1, i].GetComponent<Slot>().Card.Value)
                        {
                            Debug.Log("Line" + i + "Same:" + j);
                            SameNum++;
                        }
                        else break;
                    }
                    else break;
                }
                else break;
            }

            if (SameNum > 1)
            {
                WinNum += (int)(Casino.ResultSlots[0, i].GetComponent<Slot>().Card.Value * SameNum * Casino.WinRate * Casino.Rank);
                Debug.Log("Line" + i + "Win:");

                Debug.Log("WinNum = " + WinNum);
                for (int k = 0; k < SameNum; k++)
                {
                    Casino.ResultSlots[k, i].GetComponent<UISprite>().color = new Color(1, 0, 0);
                }
            }
        }
        return WinNum;
    }

    //Line 4,8 win
    public static int Win4_8(int line1, int index1, int line2, int index2)
    {
        if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.CT)
        {
            if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.DT == DirectionType.L && Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.DT == DirectionType.L)
            {
                if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.Value)
                {
                    Debug.Log("Line4 or Line8 Win");
                    Casino.ResultSlots[index1, line1].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index2, line2].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    return (int)(Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value * 2 * Casino.WinRate * Casino.Rank);
                }
            }
        }
        return 0;
    }

    //Line 5,6,7 win
    public static int Win5_6_7(int line1, int index1, int line2, int index2, int line3, int index3)
    {

        if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.CT)
        {
            if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.DT == DirectionType.L && Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.DT == DirectionType.L
                && Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.DT == DirectionType.L)
            {
                if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.Value)
                {
                    Debug.Log("Line5 or Line6 or Line7 Win");
                    Casino.ResultSlots[index1, line1].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index2, line2].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index3, line3].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    return (int)(Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value * 3 * Casino.WinRate * Casino.Rank);
                }
            }
        }
        return 0;
    }

    //Line 9,13 win
    public static int Win9_13(int line1, int index1, int line2, int index2)
    {

        if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.CT)
        {
            if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.DT == DirectionType.R && Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.DT == DirectionType.R)
            {
                if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.Value)
                {
                    Debug.Log("Line9 or Line13 Win");
                    Casino.ResultSlots[index1, line1].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index2, line2].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    return (int)(Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value * 2 * Casino.WinRate * Casino.Rank);
                }
            }
        }
        return 0;
    }

    //Line 10,11,12 win
    public static int Win10_11_12(int line1, int index1, int line2, int index2, int line3, int index3)
    {

        if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.CT)
        {
            if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.DT == DirectionType.R && Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.DT == DirectionType.R
                && Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.DT == DirectionType.R)
            {
                if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.Value)
                {
                    Debug.Log("Line10 or Line11 or Line12 Win");
                    Casino.ResultSlots[index1, line1].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index2, line2].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index3, line3].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    return (int)(Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value * 3 * Casino.WinRate * Casino.Rank);
                }
            }
        }
        return 0;
    }

    //Line 14 win
    public static int Win14(int line1, int index1, int line2, int index2, int line3, int index3, int line4, int index4, int line5, int index5)
    {
        if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index4, line4].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index5, line5].GetComponent<Slot>().Card.CT)
        {
            if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.DT == DirectionType.L
                && Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.DT == DirectionType.L
                && Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.DT == DirectionType.MD
                && Casino.ResultSlots[index4, line4].GetComponent<Slot>().Card.DT == DirectionType.R
                && Casino.ResultSlots[index5, line5].GetComponent<Slot>().Card.DT == DirectionType.R)
            {
                if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index4, line4].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index5, line5].GetComponent<Slot>().Card.Value)
                {
                    Debug.Log("Line14 Win");
                    Casino.ResultSlots[index1, line1].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index2, line2].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index3, line3].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index4, line4].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index5, line5].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    return (int)(Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value * 5 * Casino.WinRate * Casino.Rank);
                }
            }
        }
        return 0;
    }

    //Line 15 win
    public static int Win15(int line1, int index1, int line2, int index2, int line3, int index3, int line4, int index4, int line5, int index5)
    {
        if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index4, line4].GetComponent<Slot>().Card.CT
            && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.CT == Casino.ResultSlots[index5, line5].GetComponent<Slot>().Card.CT)
        {
            if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.DT == DirectionType.R
                && Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.DT == DirectionType.R
                && Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.DT == DirectionType.MU
                && Casino.ResultSlots[index4, line4].GetComponent<Slot>().Card.DT == DirectionType.L
                && Casino.ResultSlots[index5, line5].GetComponent<Slot>().Card.DT == DirectionType.L)
            {
                if (Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index2, line2].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index3, line3].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index4, line4].GetComponent<Slot>().Card.Value
                    && Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value == Casino.ResultSlots[index5, line5].GetComponent<Slot>().Card.Value)
                {
                    Debug.Log("Line15 Win");
                    Casino.ResultSlots[index1, line1].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index2, line2].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index3, line3].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index4, line4].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    Casino.ResultSlots[index5, line5].GetComponent<UISprite>().color = new Color(1, 0, 0);
                    return (int)(Casino.ResultSlots[index1, line1].GetComponent<Slot>().Card.Value * 5 * Casino.WinRate * Casino.Rank);
                }
            }
        }
        return 0;
    }

    //SkyWheel win
    public static int Win_SkyWheel()
    {
        int skywheelCount = 0;
        int[,] skywheel = new int[3, 2];
        int index = 0;
        for(int i = 0; i <= 4; i = i + 2)
        {
            for(int j = 0;j< 3;j++)
            {
                if (Casino.ResultSlots[i, j].GetComponent<Slot>().Card.CT == CardType.SkyWheel)
                {
                    skywheelCount++;
                    skywheel[index, 0] = i;
                    skywheel[index, 1] = j;
                    index++;
                    break;
                }
            }
        }
        if(skywheelCount == 3)
        {
            Debug.Log("SkyWheel Win");
            Casino.ResultSlots[skywheel[0, 0], skywheel[0, 1]].GetComponent<UISprite>().color = new Color(1, 0, 0);
            Casino.ResultSlots[skywheel[1, 0], skywheel[1, 1]].GetComponent<UISprite>().color = new Color(1, 0, 0);
            Casino.ResultSlots[skywheel[2, 0], skywheel[2, 1]].GetComponent<UISprite>().color = new Color(1, 0, 0);
            return (int)(int.Parse(DataManager.Cards_Card[DataManager.Cards_Card.Count - 1].Value) * 3 * Casino.WinRate * Casino.Rank);
        }
        return 0;
    }
}
