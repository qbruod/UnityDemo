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
        GameManager.Instance._packageController.ClaerItem();
        for (int i = 0; i < 4; i++)
        {
            PackageLocalItem packageLoaclItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = 2,
                type = 0,
                num = i+1,
                level = 1,
            };
            GameManager.Instance._packageController.AddItem(packageLoaclItem); // Controller负责触发事件
        }
        PackageLocalItem packageLoaclItem2 = new()
        {
            uid = Guid.NewGuid().ToString(),
            id = 4,
            type = 1,
            num = 3,
            level = 4,
            WeaponDetailText = "攻击力+" + 4 * 0.5,
        };
        GameManager.Instance._packageController.AddItem(packageLoaclItem2); // Controller负责触发事件
        PackageLocalItem packageLoaclItem3 = new()
        {
            uid = Guid.NewGuid().ToString(),
            id = 4,
            type = 1,
            num = 1,
            level = 4,
            WeaponDetailText = "攻击力+" + 4 * 0.5,
        };
        GameManager.Instance._packageController.AddItem(packageLoaclItem3); // Controller负责触发事件

    }


    //从本地读取数据
    [MenuItem("CMCmd/读取背包测试数据")]
    public static void ReadLocalPackageData()
    {
        List<PackageLocalItem> readItem=PackageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem item in readItem)
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

    [MenuItem("CMCmd/更新")]
    public static void UpDatePackagePanel()
    {
        var modifiedItem = new PackageLocalItem()
        {
            uid = PackageLocalData.Instance.item[0].uid,
            id = PackageLocalData.Instance.item[0].id,
            type = PackageLocalData.Instance.item[0].type,
            num = 12, // 修改后的值
            level = PackageLocalData.Instance.item[0].level
        };

        // 替换原有对象
        // 通过Controller调用
        GameManager.Instance._packageController.UpdateItem(modifiedItem); // Controller负责触发事件
    }
}
