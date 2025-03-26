using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image_Effect;
    public float Speed = 1;
    public Slider progressBar;
    public Text progressText;
    void Start()
    {
        progressBar.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);
        image_Effect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_Btn_LoadSceneAsync(string sceneName)
    {
        StartCoroutine(SceneLoadAsync(sceneName));
    }


    //异步加载协程
    IEnumerator SceneLoadAsync(string sceneName)
    {
        //淡入
        yield return null;
        image_Effect.gameObject.SetActive(true);
        Color tempColor=image_Effect.color;
        tempColor.a = 0;
        image_Effect.color = tempColor;
        while(image_Effect.color.a<1)
        {
            image_Effect.color += new Color(0, 0, 0, Speed * Time.deltaTime);
            yield return null;
        }

        AsyncOperation asyncOperation= SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while(!asyncOperation.isDone)
        {
            progressBar.gameObject.SetActive(true);
            progressText.gameObject.SetActive(true);
            progressBar.value=asyncOperation.progress;
            progressText.text = asyncOperation.progress*100+"%";
            if (asyncOperation.progress>=0.9f)
            {
                progressBar.value = asyncOperation.progress+0.1f;
                progressText.text = "Press space to continue";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
            
        }

    }
}
