using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpScript : MonoBehaviour
{
    public GameObject playerObject; // 玩家角色
    private Transform player; // 玩家角色的Transform
    public bool shadowdesplay = false;
    private bool isShadow = false;
    private Vector3 targetArea; // 触发区域（可以是任意区域或GameObject）
    public TextMeshProUGUI textToFade; // 需要渐变的文字（TextMeshPro）

    public float maxDistance = 5f; // 最大距离，超出这个距离文字完全透明
    public float minDistance = 2f; // 最小距离，靠近时文字完全不透明
    // Start is called before the first frame update
    void Start()
    {
        // player = playerObject.transform; // 获取玩家角色的Transform
        // isShadow = playerObject.GetComponent<PlayerBehaviour>().getState();
        targetArea = gameObject.transform.position; // 假设触发区域是当前GameObject的位置
    }

    // Update is called once per frame
    void Update()
    {
        player = playerObject.transform; // 更新玩家角色的Transform
        isShadow = playerObject.GetComponent<PlayerBehaviour>().getState();
        if (shadowdesplay && isShadow)
        {
            showText();
        }
        else if (!shadowdesplay && !isShadow)
        {
            showText();
        }
        else
        {
            // 如果不在触发区域内，隐藏文字
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 0f);
        }
        
    }
    private void showText()
    {
        float distance = Vector3.Distance(player.position, targetArea);

        // 计算透明度（距离越近，透明度越高）
        float alpha = Mathf.Clamp01((maxDistance - distance) / (maxDistance - minDistance));

        // 更新文字的透明度
        Color currentColor = textToFade.color;
        currentColor.a = alpha; // 改变文字的透明度
        textToFade.color = currentColor;
    }
}
