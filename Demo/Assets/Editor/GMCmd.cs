using System;
using System.Collections;
using System.Collections.Generic;
using test;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript
{
    [MenuItem("CMCmd/读取表格")]
    public static void ReadTable()
    {
        PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        foreach(PackageTableItem packageItem in packageTable.DataList)
        {
            Debug.Log(string.Format("【id】:{0},【name】:{1}", packageItem.id, packageItem.name)); 
        }
    }

    //创建背包数据->保存本地
    [MenuItem("CMCmd/创建背包测试数据")]
    public static void CreateLoaclPackageData()
    {
        PackageLocalData.Instance.item = new List<PackageLoaclItem>();
        for (int i = 0; i < 4; i++)
        {
            PackageLoaclItem packageLoaclItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = i,
                type = 0,
                num = i,
                level = i,

            };
            PackageLocalData.Instance.item.Add(packageLoaclItem);
        }
        PackageLoaclItem packageLoaclItem2 = new()
        {
            uid = Guid.NewGuid().ToString(),
            id = 4,
            type = 1,
            num = 3,
            level = 4,
            WeaponDetailText = "攻击力+" + 4 * 0.5,
        };
        PackageLocalData.Instance.item.Add(packageLoaclItem2);
        PackageLocalData.Instance.SavePackage();
    }


    //从本地读取数据
    [MenuItem("CMCmd/读取背包测试数据")]
    public static void ReadLocalPackageData()
    {
        List<PackageLoaclItem> readItem=PackageLocalData.Instance.LoadPackage();
        foreach (PackageLoaclItem item in readItem)
        {
            Debug.Log(item);
        }
    }

    [MenuItem("CMCmd/打开背包主界面")]
    public static void OpenPackagePanel()
    {
        Debug.Log("打开背包主界面");
        UIManager.Instance.OpenPanel(UIConst.PackageObjectPanel);
    }
}
