using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // панель паузы

    void Start()
    {
        pausePanel.SetActive(false); // скрываем панель при старте
    }

    public void PauseGame()
    {
        SFXManager.instance.PlayButtonClick();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        FindObjectOfType<MusicManager>().StopMusic(); // ?? останавливаем музыку
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        FindObjectOfType<MusicManager>().PlayMusic(); // ?? продолжаем музыку
    }
}