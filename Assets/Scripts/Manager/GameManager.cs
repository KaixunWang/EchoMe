using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("成就系统")]
    public AchievementManager achievementManager;
    
    private void Awake()
    {
        // 确保GameManager是单例，并在场景切换时不被销毁
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 如果没有成就管理器，创建一个
            if (achievementManager == null)
            {
                GameObject achievementObj = new GameObject("AchievementManager");
                achievementObj.transform.SetParent(transform);
                achievementManager = achievementObj.AddComponent<AchievementManager>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // 全局解锁成就的方法
    public void UnlockAchievement(string achievementId)
    {
        if (achievementManager != null)
        {
            achievementManager.UnlockAchievement(achievementId);
        }
    }
    
    // 获取总积分
    public int GetTotalCredit()
    {
        return achievementManager != null ? achievementManager.GetTotalCredit() : 0;
    }
    
    // 刷新成就UI（当回到成就界面时调用）
    public void RefreshAchievementUI()
    {
        if (achievementManager != null)
        {
            achievementManager.RefreshAchievementUI();
        }
    }
} 