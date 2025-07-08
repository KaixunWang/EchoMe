using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;  // 关联 PausePanel UI
    private bool isPaused = false;
    void Start()
    {
        // 确保暂停面板初始状态为隐藏
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }
    void Update()
    {
        // 按下 Esc 切换暂停状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {   
        Debug.Log("Game Paused");
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏时间
        isPaused = true;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // 恢复时间再重启
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // 恢复时间
        isPaused = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // 恢复时间再切换场景
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
