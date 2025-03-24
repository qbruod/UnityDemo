using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using test;

public class PackageCell : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler//述标对点击，进入和退出
{
    private Transform UIIcon;
    private Transform UISelect;
    private Transform UIDeleteSelect;
    private Transform UINumText;
    private Transform UISelectAni;
    private Transform UIMouseOverAni;

    //当前物品的动态数据
    private PackageLocalItem _packagrLocalData;
    //当前物品的静态数据
    private PackageTableItem PackageTableItem;
    //当前物品的父物品
    private PackagePanel uiParent;
    
    public PackageLocalItem PackageLocalItem
    {
        get { return _packagrLocalData; }
    }


    private void Awake()
    {
        InitUICell();
    }

    private void InitUICell()
    {
        UIIcon = transform.Find("Top/Image");
        UISelect = transform.Find("Select");
        UIDeleteSelect = transform.Find("DeleteSelect");
        UINumText = transform.Find("Top/Num/Text (TMP)");
        UISelectAni = transform.Find("SelectAni");
        UIMouseOverAni = transform.Find("MouseOverAni");

        UISelectAni.gameObject.SetActive(false);
        UIMouseOverAni.gameObject.SetActive(false);
    }

    ////刷新物品状态
    //public void Refresh(PackageLoaclItem packageLoaclItem,PackagePanel uiParent)
    //{
    //    //数据初始化
    //    this.packagrLocalData = packageLoaclItem;
    //    this.PackageTableItem = GameManager.Instance.GetPackageItemById(packagrLocalData.id);
    //    this.uiParent = uiParent;
    //    //更新cell中物品图片
    //    Texture2D t = (Texture2D)Resources.Load(this.PackageTableItem.imagePath);
    //    Debug.Log(this.PackageTableItem.imagePath);
    //    Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
    //    UIIcon.GetComponent<Image>().sprite = temp;
    //    //更新物品数量
    //    UINumText.GetComponent<TextMeshProUGUI>().text = packageLoaclItem.num.ToString();

    //}
    public void Refresh(PackageLocalItem packageLocalItem, PackagePanel uiParent)
    {
        // 必须初始化基础数据
        this._packagrLocalData = packageLocalItem;
        this.PackageTableItem = GameManager.Instance.GetPackageItemById(packageLocalItem.id); // 关键初始化
        this.uiParent = uiParent;

        // 空检查应放在初始化之后
        if (packageLocalItem == null || PackageTableItem == null)
        {
            Debug.LogError($"数据加载失败! 动态数据:{packageLocalItem} 静态数据:{PackageTableItem}");
            return;
        }

        // 添加空引用保护
        if (packageLocalItem == null || PackageTableItem == null)
        {
            Debug.LogError("数据加载失败，请检查物品配置");
            return;
        }

        // 安全加载图片
        Texture2D t = Resources.Load<Texture2D>(PackageTableItem.imagePath);
        if (t != null)
        {
            Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
            UIIcon.GetComponent<Image>().sprite = temp;
            if(UIManager.Instance.panelDict.ContainsKey(UIConst.PackageObjectPanel))
            {
                UINumText.GetComponent<TextMeshProUGUI>().text = UIManager.Instance.packageCountNumDict[PackageTableItem.id].ToString();
            }
            else
            {
                UINumText.GetComponent<TextMeshProUGUI>().text = packageLocalItem.num.ToString();
            }
           
        }
        else
        {
            Debug.LogWarning($"图片加载失败：{PackageTableItem.imagePath}");
        }


    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (_packagrLocalData == null || uiParent == null)
        {
            Debug.LogError("点击事件数据异常: " +
                          $"数据: {_packagrLocalData} 父面板: {uiParent}");
            return;
        }

        if (this.uiParent.chooseUid == this._packagrLocalData.uid)
        {
            return;
        }

        //选中物体与父物体显示内容 相同
        if (this.uiParent.chooseUid==this._packagrLocalData.uid)
        {
            return;
        }
        //不同,清空上一个cell动画效果，更新uid
        if (uiParent.currentSelectedItem!=null)
        {
            Transform oldCell = uiParent.currentSelectedItem.GetComponent<PackageCell>().UISelectAni;
            oldCell.gameObject.SetActive(false);
        }
        this.uiParent.chooseUid=this._packagrLocalData.uid;
        uiParent.currentSelectedItem = this.gameObject;
        //设置in，播放动画
        UISelectAni.gameObject.SetActive(true);
        UISelectAni.GetComponent<Animator>().SetTrigger("In");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIMouseOverAni.gameObject.SetActive(true);
        UIMouseOverAni.GetComponent<Animator>().SetTrigger("In");
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        UIMouseOverAni.GetComponent<Animator>().SetTrigger("Out");
        
    }
}


