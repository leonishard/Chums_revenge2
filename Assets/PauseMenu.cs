using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Assign your pause menu panel (the UI root you want to show/hide)")]
    [SerializeField] private GameObject pausePanel;

    [Header("Optional: set your main menu scene name")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        Resume(); // ensures timescale is normal if you hit play while paused in editor
    }

    private void Update()
    {
        // Toggle with Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        IsPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);

        Time.timeScale = 0f; // FREEZE GAME
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        IsPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);

        Time.timeScale = 1f; // UNFREEZE GAME
        Cursor.lockState = CursorLockMode.Locked; // change if you don't lock your cursor
        Cursor.visible = false;                   // change if you always want visible cursor
    }

    public void Restart()
    {
        Time.timeScale = 1f; // IMPORTANT before loading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; // IMPORTANT before loading
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f; // just in case
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
