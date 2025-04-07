using System.Collections;
using System.Collections.Generic;
using test;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;
    private PackageTable packageTable;
    public PackageController _packageController;
    public float walkSpeed;
    public float runSpeed;
    private void Awake()
    {
        _Instance = this;
        DontDestroyOnLoad(gameObject);//确保gameObject在场景切换时不被摧毁
        // 初始化Service
        var uiManager = UIManager.Instance;

        // 初始化Model
        var packageData = PackageLocalData.Instance;

        walkSpeed = 2f;
        runSpeed = 4f;
        // 创建Controller并注入依赖
        _packageController = new PackageController(packageData, uiManager);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // 按测Q试添加物品
        {
            AddTestItem();
        }
    }

    void AddTestItem()
    {
        var newItem = new PackageLocalItem()
        {
            uid = System.Guid.NewGuid().ToString(),
            id = 1, // 确保PackageTable中存在这个id
            type = 0,
            num = 1
        };

        // 通过Controller调用
        GameManager.Instance._packageController.AddItem(newItem); // Controller负责触发事件
        Debug.Log($"添加测试物品: {newItem}");
    }

    public static GameManager Instance
    {
        get
        {
            return _Instance;
        }
    }


    private void Start()
    {
        // 预加载静态数据
        GetPackageTable();

        // 初始化动态数据
        GetPackageLoaclData();

        // 初始化后触发首次刷新
        GameEvents.TriggerInventoryChanged(); // 触发背包数据变化事件

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
    public List<PackageLocalItem> GetPackageLoaclData()
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
    public PackageLocalItem GetPackageLoaclItemByUid(string uid)
    {
        List<PackageLocalItem> packageDataList=GetPackageLoaclData();
        foreach (PackageLocalItem item in packageDataList)
        {
            if (item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }

    //得到排序后的背包物品
    public List<PackageLocalItem> GetSortPackageLoacalData()
    { 
        List<PackageLocalItem> localItems= GetPackageLoaclData();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }


}

//背包排序方法  对比id->level->num
public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem a, PackageLocalItem b)
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

public class GameConst
{
    public const int PackageTypeFood = 0;
    public const int PackageTypeWeapon = 1;
}