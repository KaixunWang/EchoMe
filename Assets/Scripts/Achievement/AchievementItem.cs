using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItem : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;                    // 成就图标
    public TextMeshProUGUI nameText;          // 成就名称
    public TextMeshProUGUI descriptionText;   // 成就描述
    public TextMeshProUGUI scoreText;         // 积分奖励
    public Image backgroundImage;             // 背景图片
    
    [Header("Background Settings")]
    public Sprite unlockedBackgroundSprite;  // 解锁状态背景（绿色）
    public Sprite lockedBackgroundSprite;    // 未解锁状态背景（红色）
    
    [Header("Visual Settings")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = Color.gray;
    
    private Achievement currentAchievement;
    
    public void SetAchievement(Achievement achievement)
    {
        currentAchievement = achievement;
        RefreshDisplay();
    }
    
    public void RefreshDisplay()
    {
        if (currentAchievement == null) return;
        
        // 设置基本信息
        if (nameText != null)
            nameText.text = currentAchievement.name;
            
        if (descriptionText != null)
            descriptionText.text = currentAchievement.description;
            
        if (scoreText != null)
            scoreText.text = $"{currentAchievement.scoreReward}";
        
        // 设置图标
        if (iconImage != null && currentAchievement.icon != null)
        {
            iconImage.sprite = currentAchievement.icon;
            iconImage.color = currentAchievement.isUnlocked ? unlockedColor : lockedColor;
        }
        
        // 设置背景图片根据解锁状态
        if (backgroundImage != null)
        {
            if (currentAchievement.isUnlocked)
            {
                backgroundImage.sprite = unlockedBackgroundSprite; // 绿色背景
            }
            else
            {
                backgroundImage.sprite = lockedBackgroundSprite;   // 红色背景
            }
        }
        
        // 设置文本颜色
        if (nameText != null)
            nameText.color = currentAchievement.isUnlocked ? unlockedColor : lockedColor;
            
        if (descriptionText != null)
            descriptionText.color = currentAchievement.isUnlocked ? unlockedColor : lockedColor;
    }
    
    private void UpdateVisualState()
    {
        if (currentAchievement == null) return;
        
        // 根据解锁状态设置整体视觉效果
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = currentAchievement.isUnlocked ? 1f : 0.8f;
        }
    }
    
    // 点击事件处理
    public void OnItemClicked()
    {
        if (currentAchievement != null)
        {
            // 可以在这里添加点击效果
            // 比如显示详细信息、播放音效等
            Debug.Log($"点击了成就: {currentAchievement.name}");
        }
    }
}