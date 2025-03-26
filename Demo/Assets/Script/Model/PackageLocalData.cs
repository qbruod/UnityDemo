using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageLocalData
{
    private static PackageLocalData _instance;
    public List<PackageLocalItem> item;//缓存所有物品的动态信息

    public static PackageLocalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PackageLocalData();
            }
            return _instance;
        }
    }

    
    //存储背包数据到
    public void SavePackage()
    {
        string inventoryJson=JsonUtility.ToJson(this);//存储信息
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);//表格数据序列化
        PlayerPrefs.Save();
    }

    //加载背包数据
    public List<PackageLocalItem> LoadPackage()
    {
        if (item != null)
        {
            return item;
        }
        if(PlayerPrefs.HasKey("PackageLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("PackageLocalData");//文件->字符串
            PackageLocalData packageLocalData=JsonUtility.FromJson<PackageLocalData>(inventoryJson);//反序列化
            item=packageLocalData.item;
            return item;
        }
        else
        {
            item = new List<PackageLocalItem>();
            return item;
        }
    }

    //添加新物品到背包
    /// <param name="newItem">要添加的物品</param>
    public void AddItem(PackageLocalItem newItem)
    {
        item.Add(newItem);
        SavePackage();
    }

    //从背包中移除物品
    /// <param name="target">要移除的物品PackageLocalItem</param>
    public void RemoveItem(PackageLocalItem target)
    {
        item.Remove(target);
        SavePackage();
    }

    //更新背包中物品
    /// <param name="updatedItem">更新后的物品</param>
    public void UpdateItem(PackageLocalItem updatedItem)
    {
        var index = item.FindIndex(i => i.uid == updatedItem.uid);
        item[index] = updatedItem;
        SavePackage();
    }

}


[System.Serializable]
public class PackageLocalItem
{
    public string uid;
    public int id;
    public int type;
    public int num;
    //只有type=1时填写。
    public int level;
    public string WeaponDetailText= null;

    public override string ToString()
    {
        return string.Format("[id]:{0},num:{1}", id, num);
    }



}
