using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItem : MonoBehaviour
{
    [Header("UI组件")]
    public Image bgImage; // 拖拽BG节点的Image组件到这里
    public Image iconImage;
    public TextMeshProUGUI titleText; // 成就标题
    public TextMeshProUGUI descriptionText; // 成就描述
    public TextMeshProUGUI creditText; // 积分文本
    
    [Header("成就数据")]
    private string achievementId;
    private string title;
    private string description;
    private Sprite icon;
    private Sprite lockedSprite;
    private Sprite unlockedSprite;
    private int credit;
    public bool isUnlocked;

    // 设置成就数据
    public void SetAchievementData(AchievementManager.AchievementData data)
    {
        achievementId = data.id;
        title = data.title;
        description = data.description;
        icon = data.icon;
        lockedSprite = data.lockedSprite;
        unlockedSprite = data.unlockedSprite;
        credit = data.credit;
        
        // 更新UI显示
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (titleText != null)
            titleText.text = title;
        if (descriptionText != null)
            descriptionText.text = description;
        if (creditText != null)
            creditText.text = credit.ToString();
        if (iconImage != null && icon != null)
            iconImage.sprite = icon;
    }

    // 调用此方法设置状态
    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
        if (unlocked)
        {
            if (bgImage != null && unlockedSprite != null)
                bgImage.sprite = unlockedSprite;
            if (iconImage != null)
                iconImage.color = Color.white; // 设置为白色
        }
        else
        {
            if (bgImage != null && lockedSprite != null)
                bgImage.sprite = lockedSprite;
            if (iconImage != null)
                iconImage.color = new Color(0.44f, 0.44f, 0.44f, 1f); // 设置为灰色
        }
    }

    public bool GetUnlocked()
    {
        return isUnlocked;
    }

    public string GetAchievementId()
    {
        return achievementId;
    }

    // 兼容旧版本的方法
    public void SetUnlocked(bool unlocked, Sprite lockedSprite, Sprite unlockedSprite)
    {
        this.lockedSprite = lockedSprite;
        this.unlockedSprite = unlockedSprite;
        SetUnlocked(unlocked);
    }
}