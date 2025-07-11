using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("成就UI配置")]
    public GameObject achievementItemPrefab; // 成就条目的预制体
    public Transform achievementContainer; // 成就列表的父容器
    
    [Header("成就数据")]
    public List<AchievementData> achievementDataList; // 成就数据列表
    
    private List<AchievementItem> achievementItems = new List<AchievementItem>();
    public int totalCredit = 0;

    [System.Serializable]
    public class AchievementData
    {
        public string id;
        public string title;
        public string description;
        public Sprite icon;
        public Sprite lockedSprite;
        public Sprite unlockedSprite;
        public int credit;
    }

    void Start()
    {
        totalCredit = 0;
        LoadAchievementData();
        CreateAchievementUI();
        UpdateAllAchievementUI();
    }

    private void LoadAchievementData()
    {
        // 从PlayerPrefs读取所有成就状态
        foreach (var data in achievementDataList)
        {
            bool unlocked = PlayerPrefs.GetInt("Achievement_" + data.id, 0) == 1;
            if (unlocked)
                totalCredit += data.credit;
        }
        // 读取总分（防止重复加分）
        totalCredit = PlayerPrefs.GetInt("TotalCredit", totalCredit);
    }

    private void CreateAchievementUI()
    {
        if (achievementContainer == null || achievementItemPrefab == null) return;

        // 清除现有的UI
        foreach (Transform child in achievementContainer)
        {
            Destroy(child.gameObject);
        }
        achievementItems.Clear();

        // 为每个成就数据创建UI
        foreach (var data in achievementDataList)
        {
            GameObject itemObj = Instantiate(achievementItemPrefab, achievementContainer);
            AchievementItem item = itemObj.GetComponent<AchievementItem>();
            
            if (item != null)
            {
                // 设置成就数据
                item.SetAchievementData(data);
                achievementItems.Add(item);
            }
        }
    }

    private void UpdateAllAchievementUI()
    {
        foreach (var item in achievementItems)
        {
            string id = item.GetAchievementId();
            bool unlocked = PlayerPrefs.GetInt("Achievement_" + id, 0) == 1;
            item.SetUnlocked(unlocked);
        }
    }

    public void UnlockAchievement(string id)
    {
        // 保存到PlayerPrefs
        PlayerPrefs.SetInt("Achievement_" + id, 1);
        
        // 更新积分
        var data = achievementDataList.Find(x => x.id == id);
        if (data != null)
        {
            totalCredit += data.credit;
            PlayerPrefs.SetInt("TotalCredit", totalCredit);
        }
        
        PlayerPrefs.Save();
        
        // 更新UI（如果存在）
        var item = achievementItems.Find(x => x.GetAchievementId() == id);
        if (item != null && !item.GetUnlocked())
        {
            item.SetUnlocked(true);
        }
    }

    public int GetTotalCredit()
    {
        return PlayerPrefs.GetInt("TotalCredit", 0);
    }

    // 刷新成就UI（当回到成就界面时调用）
    public void RefreshAchievementUI()
    {
        if (achievementContainer != null)
        {
            CreateAchievementUI();
            UpdateAllAchievementUI();
        }
    }
} 