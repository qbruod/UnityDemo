using System.Collections;
using System.Collections.Generic;
using test;
using UnityEngine;
using static UnityEditor.Progress;

namespace Test
{
    public class BasePanel : MonoBehaviour
    {
        
        protected bool isRemove=false;//当前页面是否被关闭
        protected new string name;//页面名称

        public virtual void OpenPanel(string name)
        {
            this.name = name;
            gameObject.SetActive(true);
        }

        public virtual void ClosePanel(string name)
        {
            Debug.Log("444");

            if (UIManager.Instance.panelDict.ContainsKey(name))
            {
                isRemove = true;
                gameObject.SetActive(false);
                Destroy(gameObject);
                UIManager.Instance.panelDict.Remove(name);
                List<PackageLocalItem> readItem = PackageLocalData.Instance.LoadPackage();

                UIManager.Instance.ClearPackageCellDict();


                
            }
        }
    }

}
