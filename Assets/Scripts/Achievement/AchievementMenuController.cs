using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Transform achievementItemParent;
    public GameObject achievementItemPrefab;
    
    [Header("Category Selection - Manual Setup")]
    public Button[] categoryButtons; // 手动拖入你的4个按钮
    public string[] categoryNames = {"General","Control","Progress","Other"}; // 对应按钮的分类名称
    private string selectedCategory = "";
    
    [Header("Achievement Display")]
    public TextMeshProUGUI totalScoreText; // 显示总积分
    
    private List<AchievementItem> achievementItems = new List<AchievementItem>();
    
    private void Start()
    {
        SetupCategoryButtons();
        
        // 订阅成就事件
        AchievementManager.OnAchievementUnlocked += OnAchievementUnlocked;

    }
    
    private void SetupCategoryButtons()
    {
        Debug.Log("设置分类按钮开始");
        if (categoryButtons == null || categoryButtons.Length == 0) 
        {
            Debug.LogError("categoryButtons 数组为空！");
            return;
        }
        
        Debug.Log($"找到 {categoryButtons.Length} 个按钮");
        
        // 为每个按钮设置点击事件
        for (int i = 0; i < categoryButtons.Length && i < categoryNames.Length; i++)
        {
            int index = i; // 闭包变量
            string categoryName = categoryNames[index];
            
            Debug.Log($"设置按钮 {index}: {categoryName}");
            
            // 清除现有监听器并添加新的
            categoryButtons[index].onClick.RemoveAllListeners();
            categoryButtons[index].onClick.AddListener(() => {
                Debug.Log($"按钮被点击: {categoryName}");
                SelectCategory(categoryName);
            });
            
            // 设置按钮文本（如果有TextMeshProUGUI组件）
            TextMeshProUGUI buttonText = categoryButtons[index].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = categoryName;
            }
        }
        
        // 默认选择第一个分类
        if (categoryNames.Length > 0)
        {
            Debug.Log($"默认选择分类: {categoryNames[0]}");
            SelectCategory(categoryNames[0]);
        }
    }
    
    private void OnDestroy()
    {
        // 取消订阅
        AchievementManager.OnAchievementUnlocked -= OnAchievementUnlocked;
    }
    
    public void SelectCategory(string category)
    {
        Debug.Log($"SelectCategory 被调用: {category}");
        selectedCategory = category;
        DisplayAchievements();
        UpdateTotalScore();
        
        // 更新按钮选中状态
        UpdateButtonStates();
    }
    
    private void UpdateButtonStates()
    {
        for (int i = 0; i < categoryButtons.Length && i < categoryNames.Length; i++)
        {
            // 可以在这里添加选中状态的视觉效果
            // 比如改变颜色、添加边框等
            if (categoryNames[i] == selectedCategory)
            {
                // 选中状态
                categoryButtons[i].interactable = false; // 或者改变颜色
            }
            else
            {
                // 未选中状态
                categoryButtons[i].interactable = true;
            }
        }
    }
    
    private void UpdateTotalScore()
    {
        if (totalScoreText != null && AchievementManager.Instance != null)
        {
            int totalScore = AchievementManager.Instance.GetTotalScore();
            totalScoreText.text = $"Total Score:\n {totalScore}";
        }
    }
    
    private void DisplayAchievements()
    {
        if (AchievementManager.Instance == null) return;
        
        Debug.Log($"当前选中分类: {selectedCategory}");
        Debug.Log($"总成就数量: {AchievementManager.Instance.achievements.Count}");
        
        // 清除现有成就项
        foreach (var item in achievementItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        achievementItems.Clear();
        
        // 获取选中分类的成就
        List<Achievement> categoryAchievements = AchievementManager.Instance.GetAchievementsByCategory(selectedCategory);
        Debug.Log($"分类 '{selectedCategory}' 下的成就数量: {categoryAchievements.Count}");
        
        foreach (var achievement in categoryAchievements)
        {
            Debug.Log($"成就: {achievement.name}, 分类: {achievement.category}");
        }
        
        // 创建成就项
        foreach (var achievement in categoryAchievements)
        {
            GameObject itemObj = Instantiate(achievementItemPrefab, achievementItemParent);
            AchievementItem item = itemObj.GetComponent<AchievementItem>();
            
            if (item != null)
            {
                item.SetAchievement(achievement);
                achievementItems.Add(item);
            }
        }
    }
    
    private void OnAchievementUnlocked(Achievement achievement)
    {
        // 如果解锁的成就属于当前选中的分类，刷新显示
        if (achievement.category == selectedCategory)
        {
            RefreshAchievementItems();
        }
        
        // 总是更新总积分显示
        UpdateTotalScore();
    }
    
    private void RefreshAchievementItems()
    {
        foreach (var item in achievementItems)
        {
            if (item != null)
                item.RefreshDisplay();
        }
    }
    
    // 公共方法供外部调用
    public void RefreshAll()
    {
        DisplayAchievements();
        UpdateTotalScore();
    }
}