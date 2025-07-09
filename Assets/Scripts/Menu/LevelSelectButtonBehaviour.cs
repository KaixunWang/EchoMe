using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtonBehaviour : MonoBehaviour
{
    [Header("按钮设置")]
    public int levelIndex = 1; // 关卡索引
    public Sprite normalSprite; // 正常状态的图片
    public Sprite pressedSprite; // 按下状态的图片
    public Sprite lockedSprite; // 锁定状态的图片
    
    [Header("UI组件")]
    public Image buttonImage; // 按钮图片组件
    
    private bool isUnlocked = false; // 是否解锁
    private bool isPressed = false; // 是否被按下
    
    void Start()
    {
        // 获取组件引用
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
            
        // 检查关卡是否解锁
        CheckLevelUnlock();
        
        // 设置初始图片
        UpdateButtonImage();
    }
    
    // 检查关卡是否解锁
    void CheckLevelUnlock()
    {
        // 这里可以根据你的游戏存档系统来判断关卡是否解锁
        // 例如：isUnlocked = PlayerPrefs.GetInt("Level_" + levelIndex + "_Unlocked", 0) == 1;
        isUnlocked = levelIndex <= 0; // 临时：前0关默认解锁
    }
    
    // 更新按钮图片
    void UpdateButtonImage()
    {
        if (buttonImage == null) return;
        
        if (!isUnlocked)
        {
            buttonImage.sprite = lockedSprite;
        }
        else if (isPressed)
        {
            buttonImage.sprite = pressedSprite;
        }
        else
        {
            buttonImage.sprite = normalSprite;
        }
    }
    
    // 点击进入关卡
    public void OnClick()
    {
        if (!isUnlocked)
        {
            Debug.Log("关卡 " + levelIndex + " 尚未解锁！");
            return;
        }
        
        Debug.Log("进入关卡 " + levelIndex);
        
        // 通知LevelSelectManager
        LevelSelectManager.Instance?.OnLevelButtonClicked(levelIndex);
        
        // 设置按下状态
        SetPressed(true);
    }
    
    // 设置按下状态
    public void SetPressed(bool pressed)
    {
        isPressed = pressed;
        UpdateButtonImage();
    }
    
    // 解锁关卡
    public void UnlockLevel()
    {
        isUnlocked = true;
        UpdateButtonImage();
    }
}
