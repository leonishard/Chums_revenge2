using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Assign your pause UI root (Panel or Canvas group)")]
    [SerializeField] private GameObject pausePanel;

    [Header("Main Menu scene name (must be in Build Settings)")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        // Cursor free for topdown games
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pausePanel) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        IsPaused = true;
        if (pausePanel) pausePanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        IsPaused = false;
        if (pausePanel) pausePanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        if (pausePanel) pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // IMPORTANT: wipe run stats BEFORE reloading the scene
        if (GameManager.I != null)
            GameManager.I.ResetRun();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        if (pausePanel) pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // IMPORTANT: wipe run stats BEFORE going to menu
        if (GameManager.I != null)
            GameManager.I.ResetRun();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        if (pausePanel) pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
