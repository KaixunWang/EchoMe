using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [Header("关卡设置")]
    public string levelScenePrefix = "Level_"; // 关卡场景名称前缀
    public int totalLevels = 10; // 总关卡数
    
    [Header("UI管理")]
    public LevelSelectButtonBehaviour[] levelButtons; // 关卡按钮数组
    
    // 单例模式
    public static LevelSelectManager Instance { get; private set; }
    
    // 当前选中的关卡
    private int selectedLevel = 0;
    
    void Awake()
    {
        // 设置单例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InitializeLevelButtons();
        LoadUnlockedLevels();
    }
    
    // 初始化关卡按钮
    void InitializeLevelButtons()
    {
        // 如果没有手动设置按钮数组，自动查找所有按钮
        if (levelButtons == null || levelButtons.Length == 0)
        {
            levelButtons = FindObjectsOfType<LevelSelectButtonBehaviour>();
        }
        
        // 为每个按钮设置关卡索引
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                levelButtons[i].levelIndex = i + 1;
            }
        }
        
        Debug.Log("找到 " + levelButtons.Length + " 个关卡按钮");
    }
    
    // 加载已解锁的关卡
    void LoadUnlockedLevels()
    {
        // 这里可以从存档系统加载已解锁的关卡
        // 例如：int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
    }
    
    // 关卡按钮被点击时的处理
    public void OnLevelButtonClicked(int levelIndex)
    {
        selectedLevel = levelIndex;
        Debug.Log("选中关卡: " + levelIndex);
        
        // 可以在这里添加确认对话框、音效等
        
        // 加载关卡场景
        LoadLevel(levelIndex);
    }
    
    // 加载关卡
    void LoadLevel(int levelIndex)
    {
        string sceneName = levelScenePrefix + levelIndex;
        
        Debug.Log("尝试加载关卡: " + sceneName);
        
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("无法加载场景: " + sceneName + "。请确保场景已添加到Build Settings中。错误: " + e.Message);
        }
    }
    
    // 解锁关卡
    public void UnlockLevel(int levelIndex)
    {
        // 保存到存档系统
        PlayerPrefs.SetInt("Level_" + levelIndex + "_Unlocked", 1);
        PlayerPrefs.Save();
        
        // 更新按钮状态
        foreach (var button in levelButtons)
        {
            if (button != null && button.levelIndex == levelIndex)
            {
                button.UnlockLevel();
                break;
            }
        }
    }
    
    // 返回主菜单
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // 替换为你的主菜单场景名
    }
    
    // 重置所有关卡进度
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("进度已重置");
        
        // 重新加载场景以更新按钮状态
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
