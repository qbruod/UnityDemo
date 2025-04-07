using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using test;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Unity.VisualScripting;

public class PackageDetail : MonoBehaviour
{
    private Transform UIIcon;
    private Transform UITitleText;
    private Transform UIDetailText;
    private Transform UIUsingBtn;

    private PackageLocalItem packageLoaclData;
    private PackageTableItem packageTableItem;

    private PackagePanel uiParent;
    int? AddBlood =null;
    int? SpeedUp =null;
    int? AddATK=null;

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
        UIUsingBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnBtnUsing);
    }

    private void OnBtnUsing()
    {
        if (UIManager.Instance.packageCountNumDict[packageLoaclData.id]>0)
        {
            var newItem = new PackageLocalItem()
            {
                uid = System.Guid.NewGuid().ToString(),
                id = packageLoaclData.id, // 确保PackageTable中存在这个id
                type = packageLoaclData.type,
                num = -1
            };
            // 通过Controller调用
            GameManager.Instance._packageController.AddItem(newItem); // Controller负责触发事件
            GameEvents.TriggerInventoryChanged();
            Debug.Log(111);
            if (AddBlood != null)
            {
                Debug.Log("Blood");
                GameEvents.TriggerBtnBlood(AddBlood);
            }
            else if (SpeedUp != null)
            {
                Debug.Log("速度");
                GameEvents.TriggerBtnSpeed(SpeedUp);
            }
            else if (AddATK != null)
            {
                Debug.Log("ATK");
                GameEvents.TriggerBtnAtk(AddATK);
            }

        }
        else
        {
            Debug.Log("库存不足");
        }

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
        //如果当前是页面是Food
        if (packageLoaclData.type == GameConst.PackageTypeFood && UIManager.Instance.panelDict.ContainsKey(UIConst.PackageObjectPanel))
        {
            UIDetailText.GetComponent<TextMeshProUGUI>().text = packageTableItem.detailDescription;
            string description=packageTableItem.detailDescription;
            
            if (description != null && description.IndexOf("生命") != -1)
            {
                //正则表达式获取加血信息
                AddBlood = int.Parse(System.Text.RegularExpressions.Regex.Replace(description, @"[^0-9]+", ""));
            }
            else if (description != null && description.IndexOf("速度") != -1)
            {
                //获取加速信息
                SpeedUp = int.Parse(System.Text.RegularExpressions.Regex.Replace(description, @"[^0-9]+", ""));
            }
        }
        //如果当前页面是武器
        else if (packageLoaclData.type == GameConst.PackageTypeWeapon && UIManager.Instance.panelDict.ContainsKey(UIConst.PackageWeaponPanel))
        {
            Transform LevelText = transform.Find("Top/ObjectLevel");
            UIDetailText.GetComponent<TextMeshProUGUI>().text = packageLoaclData.WeaponDetailText;
            LevelText.GetComponent<TextMeshProUGUI>().text = "等级：" + packageLoaclData.level.ToString();
            string description = packageLoaclData.WeaponDetailText;
            if (description != null && description.IndexOf("攻击力") == 1)
            {
                //正则表达式获取加血信息
                AddATK = int.Parse(System.Text.RegularExpressions.Regex.Replace(description, @"[^0-9]+", ""));
            }
        } 
    }
}
