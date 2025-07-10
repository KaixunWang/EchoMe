using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            
        // 自动设置按钮点击事件
        SetupButtonClick();
            
        // 检查关卡是否解锁
        CheckLevelUnlock();
        
        // 设置初始图片
        UpdateButtonImage();
    }
    
    // 自动设置按钮点击事件
    void SetupButtonClick()
    {
        // 获取Button组件
        Button button = GetComponent<Button>();
        if (button != null)
        {
            // 清除现有的点击事件
            button.onClick.RemoveAllListeners();
            
            // 添加新的点击事件
            button.onClick.AddListener(OnClick);
            
            Debug.Log($"已为关卡 {levelIndex} 按钮设置点击事件");
        }
        else
        {
            Debug.LogError($"关卡 {levelIndex} 按钮缺少Button组件！");
        }
    }
    
    // 检查关卡是否解锁
    void CheckLevelUnlock()
    {
        // 这里可以根据你的游戏存档系统来判断关卡是否解锁
        // 例如：isUnlocked = PlayerPrefs.GetInt("Level_" + levelIndex + "_Unlocked", 0) == 1;
        
        // 确保第0关默认解锁
        if (levelIndex == 0)
        {
            isUnlocked = true;
        }
        else
        {
            // 其他关卡可以根据需要设置解锁条件
            isUnlocked = levelIndex <= 1; // 临时：前1关默认解锁
        }
        
        Debug.Log($"关卡 {levelIndex} 解锁状态: {isUnlocked}");
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
        Debug.Log($"按钮被点击！关卡 {levelIndex}");
        
        if (!isUnlocked)
        {
            Debug.Log("关卡 " + levelIndex + " 尚未解锁！");
            return;
        }
        
        Debug.Log("进入关卡 " + levelIndex);
        
        // 通知LevelSelectManager
        if (LevelSelectManager.Instance != null)
        {
            LevelSelectManager.Instance.OnLevelButtonClicked(levelIndex);
        }
        else
        {
            Debug.LogError("LevelSelectManager.Instance 为空！");
        }
        
        // 设置按下状态
        SetPressed(true);
    }
    
    // 鼠标点击检测（备用方法）
    void OnMouseDown()
    {
        Debug.Log($"鼠标点击检测到！关卡 {levelIndex}");
        OnClick();
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
