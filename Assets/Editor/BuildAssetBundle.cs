using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class BuildAssetBundle : Editor {

    //在编辑器模式下生成asset文件，供正式环境读取
    [MenuItem("Assets/Create ResourceData")]
    public static void ExcuteBuild()
    {
        //Create Cards.asset
        Cards_Excel holder1 = ScriptableObject.CreateInstance<Cards_Excel>();

        holder1.Card = ExcelAccess.SelectCardsTable(1);

        AssetDatabase.CreateAsset(holder1, HolderPath(ExcelAccess.CARDS));
        AssetImporter import1 = AssetImporter.GetAtPath(HolderPath(ExcelAccess.CARDS));
        import1.assetBundleName = ExcelAccess.CARDS;

        Debug.Log("BuildAsset Success!");
    }

    public static string HolderPath(string holderName)
    {
        return "Assets/Resources/Datas/" + holderName + ".asset";
    }
}
