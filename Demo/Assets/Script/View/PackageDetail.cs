using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using test;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PackageDetail : MonoBehaviour
{
    private Transform UIIcon;
    private Transform UITitleText;
    private Transform UIDetailText;
    private Transform UIUsingBtn;

    private PackageLocalItem packageLoaclData;
    private PackageTableItem packageTableItem;

    private PackagePanel uiParent;

    private void Awake()
    {
        InitUIName();
        
    }

    private void CleanPackageDetail()
    {
        Transform UIScrollView = transform.Find("Center/Scroll View");
        //清空滚动容器中原本的物体
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
    }

    private void InitUIName()
    {
        UIIcon = transform.Find("Top/ObjectIcon");
        UITitleText = transform.Find("Top/ObjectTitleText (TMP)");
        UIDetailText = transform.Find("Bottom/ObjectDetailText (TMP)");
        UIUsingBtn = transform.Find("Bottom/UsingButton");

    }

    //刷新详情界面
    public void Refresh(PackageLocalItem packageLoaclData, PackagePanel uiParent)
    {
        //初始化信息
        this.uiParent = uiParent;
        this.packageTableItem = GameManager.Instance.GetPackageItemById(packageLoaclData.id);
        this.packageLoaclData = packageLoaclData;

        //图片加载
        Texture2D t = (Texture2D)Resources.Load(this.packageTableItem.imagePath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;

        //名称加载
        UITitleText.GetComponent<TextMeshProUGUI>().text = packageTableItem.name;

        //详情加载
        if (packageLoaclData.type == GameConst.PackageTypeFood && UIManager.Instance.panelDict.ContainsKey(UIConst.PackageObjectPanel))
        {
            UIDetailText.GetComponent<TextMeshProUGUI>().text = packageTableItem.detailDescription;

        }
        else if (packageLoaclData.type == GameConst.PackageTypeWeapon && UIManager.Instance.panelDict.ContainsKey(UIConst.PackageWeaponPanel))
        {
            Transform LevelText = transform.Find("Top/ObjectLevel");
            UIDetailText.GetComponent<TextMeshProUGUI>().text = packageLoaclData.WeaponDetailText;
            LevelText.GetComponent<TextMeshProUGUI>().text = "等级：" + packageLoaclData.level.ToString();
        } 
    }
}
