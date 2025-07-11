using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject achievementPanel;
    public void Start()
    {
        // 初始化菜单或其他设置
        Debug.Log("MenuManager initialized");
    }

    // 退出游戏（打包后有效）
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
    
    // 返回主菜单（可用于暂停菜单中）
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    // 显示成就菜单
    public void ShowAchievements()
    {
        Debug.Log("显示成就菜单");
        //将achievementpanel设置为active
        achievementPanel.SetActive(true);
    }

    public void CloseAchievements()
    {
        achievementPanel.SetActive(false);
    }
}
