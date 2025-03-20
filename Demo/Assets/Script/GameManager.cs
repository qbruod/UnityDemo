using System.Collections;
using System.Collections.Generic;
using test;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;
    private PackageTable packageTable;
    public static GameManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Awake()
    {
        _Instance = this;
        DontDestroyOnLoad(gameObject);//确保gameObject在场景切换时不被摧毁
    }

    private void Start()
    {
        // 预加载静态数据
        GetPackageTable();

        // 初始化动态数据
        GetPackageLoaclData();

        UIManager.Instance.OpenPanel(UIConst.PackageObjectPanel);
    }

    //加载静态数据
    public PackageTable GetPackageTable()
    {
        if (packageTable == null)
        {
            packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        }
        return packageTable;
    }

    //加载动态数据
    public List<PackageLoaclItem> GetPackageLoaclData()
    {
        return PackageLocalData.Instance.LoadPackage();
    }

    //根据id拿到静态数据指定项
    public PackageTableItem GetPackageItemById(int id)
    {
        List<PackageTableItem> pickageDataList= GetPackageTable().DataList;
        foreach (PackageTableItem item in pickageDataList)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }


    //根据uid拿到动态数据指定项
    public PackageLoaclItem GetPackageLoaclItemByUid(string uid)
    {
        List<PackageLoaclItem> packageDataList=GetPackageLoaclData();
        foreach (PackageLoaclItem item in packageDataList)
        {
            if (item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }

    //得到排序后的背包物品
    public List<PackageLoaclItem> GetSortPackageLoacalData()
    { 
        List<PackageLoaclItem> localItems= GetPackageLoaclData();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }


}

//背包排序方法  对比id->level->num
public class PackageItemComparer : IComparer<PackageLoaclItem>
{
    public int Compare(PackageLoaclItem a, PackageLoaclItem b)
    {
        PackageTableItem x=GameManager.Instance.GetPackageItemById(a.id);
        PackageTableItem y=GameManager.Instance.GetPackageItemById(b.id);
        int idComparison=y.id.CompareTo(x.id);
        if (idComparison == 0)
        {
            int levelComparison=b.level.CompareTo(a.level);
            if (levelComparison == 0)
            {
                return b.num.CompareTo(a.num);
            }
            return levelComparison;
        }
        return idComparison;
    }
}