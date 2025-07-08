using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        // 确保场景控制器唯一，并在切换场景时不被销毁
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 保留跨场景
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 加载指定场景（同步）
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 异步加载指定场景（推荐用于Loading场景）
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading... " + (asyncLoad.progress * 100f) + "%");
            yield return null;
        }

        Debug.Log("Scene loaded: " + sceneName);
    }

    // 重新加载当前场景
    public void ReloadCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // 退出游戏（仅在PC/桌面端有效）
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
