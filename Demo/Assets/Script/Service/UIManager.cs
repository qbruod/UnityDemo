using System.Collections;
using System.Collections.Generic;
using Test;
using UnityEngine;

namespace test
{
    public class UIManager
    {
        private static UIManager _instance;
        //界面配置路径表
        private Dictionary<string, string> pathDict;

        private Transform _uiRoot;
        //预制件缓存字典
        private Dictionary<string, GameObject> prefabDict;
        //已打开界面缓存字典
        public Dictionary<string, BasePanel> panelDict;
        //PackageCell缓存字典
        public Dictionary<int,PackageCell> packageCellIdDict;
        //每个id对应的num总数
        public Dictionary<int,int> packageCountNumDict;

        public UIManager() 
        {
            this.InitDicts();
            // 初始化时订阅全局事件
            GameEvents.OnPanelOpened += HandlePanelOpen;
            GameEvents.OnInventoryDataChanged += HandleGlobalInventoryChange;
        }
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIManager();
                }
                
                 return _instance;
                
            }
        }

        //懒加载UIRoot
        public Transform UIRoot
        {
            get
            {
                if (this._uiRoot == null)
                {
                    this._uiRoot = GameObject.Find("Canvas").transform; //查找场景中名为 "Canvas" 的游戏对象，并获取它的 Transform 组件，赋值给 _uiRoot
                    
                }
                return this._uiRoot;
            }
        }
        private void InitDicts()
        {
            prefabDict=new Dictionary<string, GameObject>();
            panelDict=new Dictionary<string, BasePanel>();
            packageCellIdDict = new Dictionary<int, PackageCell>();
            packageCountNumDict=new Dictionary<int, int>();
            pathDict = new Dictionary<string, string>()
            {
                //界面名称，界面路径
                {UIConst.PackageObjectPanel, "Prefab/Panel/Package/PackageObjectPanel"},
                {UIConst.PackageWeaponPanel,"Prefab/Panel/Package/PackageWeaponPanel" }
            };
        }

        //打开界面
        public BasePanel OpenPanel(string name)
        {
            BasePanel panel = null;
            if(panelDict.TryGetValue(name, out panel))
            {
                Debug.LogError("界面已打开：" + name);
                panel= panelDict[name];
                panel.gameObject.SetActive(true);
                return panel;
            }

            string path = "";
            if(!pathDict.TryGetValue(name, out path))
            {
                Debug.LogError("界面名称错误，或未配置路径：" + name);
                return null;
            }

            //使用缓存的预制件
            GameObject panelPrefab = null;
            if(!prefabDict.TryGetValue(name, out panelPrefab))
            {
                string realPath = "Prefab/Panel/Package/" + name;
                panelPrefab=Resources.Load<GameObject>(realPath);
                prefabDict.Add(name, panelPrefab);
            }

            if (UIRoot.Find(name))
            {
                Transform inactivationPanel = UIRoot.Find(name);
                panel = inactivationPanel.GetComponent<BasePanel>();
                panel.gameObject.SetActive(true);
                panelDict.Add(name, panel);
                return panel;
            }
            else
            {
                //打开界面
                GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);//实例化界面
                panel = panelObject.GetComponent<BasePanel>();
                panelDict.Add(name, panel);
                return panel;
            }
        }

        //关闭界面
        public bool ClosePanel(string name)
        {
            BasePanel panel=null;
            if(!panelDict.TryGetValue(name, out panel))
            {
                Debug.LogError("界面未打开" + name);
                return false;
            }
            
            panel.ClosePanel(name);
            return true;
        }

        public void AddPackageCellDict(PackageLocalItem packageLocalItem,PackageCell packageCell)
        {
            packageCellIdDict.Add(packageLocalItem.id, packageCell);  
        }

        public void ClearPackageCellDict()
        {
            packageCellIdDict.Clear();
            packageCountNumDict.Clear();
        }

        /// <summary>
        /// 处理面板打开事件
        /// </summary>
        /// <param name="panelName">被打开的面板名称</param>
        private void HandlePanelOpen(string panelName)
        {
            if (panelName == UIConst.PackageObjectPanel)
            {
                Time.timeScale = 0f; // 暂停游戏时间
            }
        }

        /// <summary>
        /// 处理全局背包数据变化事件
        /// </summary>
        private void HandleGlobalInventoryChange()
        {
            // 自动刷新所有打开的背包相关面板
            foreach (var panel in panelDict.Values)
            {
                if (panel is PackagePanel)
                {
                    (panel as PackagePanel).RefreshUI(); // 刷新UI
                }
            }
        }

    }





    //存储名称的 常量表
    public class UIConst
    {
        public const string PackageObjectPanel = "PackageObjectPanel";

        public const string PackageWeaponPanel = "PackageWeaponPanel";
    }
}
