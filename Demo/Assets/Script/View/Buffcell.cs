using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buffcell : MonoBehaviour
{
    private Transform buffIcon;
    private PlayerMove playerMove;
    private float originalWalkSpeed;
    private float originalRunSpeed;

    private void Awake()
    {

        InitBuffCell();
        GameEvents.OnUsingSpeed += BuffUpdate;
        // 获取PlayerMove组件
        playerMove = FindObjectOfType<PlayerMove>(); ;
        buffIcon.gameObject.SetActive(false);
        if (playerMove == null)
        {
            Debug.LogError("PlayerMove component not found!");
        }
        else
        {
            // 初始化原始速度
            originalWalkSpeed = 2f;
            originalRunSpeed = 5f;
        }
    }

    private void InitBuffCell()
    {
        buffIcon = transform.Find("Image");

    }



    private IEnumerator Refrsh(float speedMultiplier)
    {
        // 加载纹理
        Texture2D t = Resources.Load<Texture2D>("Image/UI/BuffIcon/wingfoot");
        if (t != null)
        {
            buffIcon.gameObject.SetActive(true);
            Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
            buffIcon.GetComponent<UnityEngine.UI.Image>().sprite = temp;
        }

        // 加速玩家移动
        if (playerMove != null)
        {
            playerMove.walkSpeed *= speedMultiplier;
            playerMove.runSpeed *= speedMultiplier;
        }

        // 等待十秒（真实时间）
        yield return new WaitForSecondsRealtime(10);

        // 恢复原始速度
        if (playerMove != null)
        {
            playerMove.walkSpeed = originalWalkSpeed;
            playerMove.runSpeed = originalRunSpeed;
        }

        // 清除buff图标
        buffIcon.gameObject.SetActive(false);
    }

    private void BuffUpdate(int? SpeedUp)
    {
        // 设置默认加速倍数为1.5倍
        float speedMultiplier = 2f;

        StartCoroutine(Refrsh(speedMultiplier));
    }
}
