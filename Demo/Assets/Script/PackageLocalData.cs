using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageLocalData
{
    private static PackageLocalData _instance;
    public List<PackageLoaclItem> item;//缓存所有物品的动态信息

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

    
    public void SavePackage()
    {
        string inventoryJson=JsonUtility.ToJson(this);//存储信息
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);//表格数据序列化
        PlayerPrefs.Save();
    }

    public List<PackageLoaclItem> LoadPackage()
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
            item = new List<PackageLoaclItem>();
            return item;
        }
    }
}


[System.Serializable]
public class PackageLoaclItem
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
