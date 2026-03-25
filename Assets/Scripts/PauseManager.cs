using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] FirstPersonController controller;
    [SerializeField] GameObject PauseMenu;

    public UnityEvent OnPause = new();
    public UnityEvent OnResume = new();

    public static bool paused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!paused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("PAUSE");

        paused = true;
        Time.timeScale = 0f;

        controller.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenu.SetActive(true);

        OnPause.Invoke();
    }

    public void ResumeGame()
    {
        Debug.Log("RESUME");

        paused = false;
        Time.timeScale = 1f;

        controller.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.SetActive(false);

        OnResume.Invoke();
    }

    public void Restart()
    {
        paused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        paused = false;
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Main Menu");
    }
}
