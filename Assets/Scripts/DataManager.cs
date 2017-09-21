using UnityEngine;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {

    //供外部调取的数据，细化到标签页
    
    public static IList<Cards_Sheet> Cards_Card;

    //内部读数据需要用到的属性
    private static string[] assetNames = { "Cards" };

    public static void ReadDatas()
    {
        Cards_Card = (Resources.Load<Object>("Datas/" + assetNames[0]) as Cards_Excel).Card;
    }
    
}
