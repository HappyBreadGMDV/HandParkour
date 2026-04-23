using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public FirstPersonController PlayerController;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        PlayerController.cameraCanMove = true;
        PlayerController.lockCursor = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        PlayerController.cameraCanMove = false;
        PlayerController.lockCursor = true;
        Time.timeScale = 0f;
        isPaused = true;
    }
}
