using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using System.Collections.Generic;

public class ExcelAccess
{
    //存储所有Excel文件名
    public const string CARDS = "Cards";

    //存储所有页签名
    public static string[] CARDS_SheetNames = { "Card" };

    //读Cards.xlsx表
    
    public static List<Cards_Sheet> SelectCardsTable(int tableId)
    {
        DataRowCollection collect = ReadExcel(CARDS + ".xlsx", CARDS_SheetNames[tableId - 1]);
        List<Cards_Sheet> array = new List<Cards_Sheet>();

        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            Cards_Sheet card = new Cards_Sheet();
            //
            Debug.Log(collect[i][3].ToString());
            card.ID = collect[i][0].ToString();
            card.Value = collect[i][1].ToString();
            card.DirectionType = collect[i][2].ToString();
            card.CardType = collect[i][3].ToString();
            card.ImgName = collect[i][4].ToString();
            card.Weight_1 = collect[i][5].ToString();
            card.Weight_2 = collect[i][6].ToString();
            card.Weight_3 = collect[i][7].ToString();
            card.Weight_4 = collect[i][8].ToString();
            card.Weight_5 = collect[i][9].ToString();

            array.Add(card);
        }
        return array;
    }

    /// <summary>
    /// 读取excel的sheet下的内容
    /// </summary>
    /// <param name="excelName"></param>
    /// <param name="sheet"></param>
    /// <returns></returns>
    static DataRowCollection ReadExcel(string excelName,string sheet)
    {
        FileStream stream = File.Open(FilePath(excelName), FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //int columns = result.Tables[0].Columns.Count;
        //int rows = result.Tables[0].Rows.Count;
        Debug.Log("excelName = " + excelName);
        return result.Tables[sheet].Rows;
    }

    public static string FilePath(string name)
    {
        return Application.dataPath+"/OriginalDatas/" + name;
    }

}

