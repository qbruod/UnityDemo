using System.Collections;
using System.Collections.Generic;
using test;
using UnityEngine;

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
            isRemove = true;
            gameObject.SetActive(false);
            Destroy(gameObject);

            if (UIManager.Instance.panelDict.ContainsKey(name))
            {
                UIManager.Instance.panelDict.Remove(name);
            }
        }
    }

}
