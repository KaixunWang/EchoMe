using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class WinScript : MonoBehaviour
{
    public GameObject oneStar;  // 1star
    public GameObject twoStar;  // 2star
    public GameObject threeStar; // 3star
    public TMP_Text winPanel; // 胜利面板
    // Start is called before the first frame update
    void Start()
    {
        SetStars(0); // 初始化时隐藏所有星星
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetStars(int score)
    {
        // 隐藏所有星星
        oneStar.SetActive(false);
        twoStar.SetActive(false);
        threeStar.SetActive(false);
        // winPanel.setActive(true); // 隐藏胜利面板

        // 根据分数显示对应的星星
        if (score >= 1) oneStar.SetActive(true);  // 激活1颗星
        if (score >= 2) twoStar.SetActive(true);  // 激活2颗星
        if (score >= 3) threeStar.SetActive(true);  // 激活3颗星
    }
    public void ShowWinPanel(string message)
    {
        // 使用正则表达式判断当前场景是否为 Level_x 格式
        string currentSceneName = SceneManager.GetActiveScene().name;
        Match match = Regex.Match(currentSceneName, @"^Level_(\d+)$");
        
        if (match.Success)
        {
            string levelNumber = match.Groups[1].Value;
            string achievementName = $"PassLevel{levelNumber}";
            AchievementManager.Instance.UnlockAchievement(achievementName);
        }
        
        winPanel.text = message; // 设置胜利面板的文本
    }
}
