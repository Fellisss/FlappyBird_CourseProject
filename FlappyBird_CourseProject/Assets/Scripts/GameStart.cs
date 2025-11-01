using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject startPanel;

    void Start()
    {
        // Пауза игры в начале
        Time.timeScale = 0;

        // Показываем панель старта
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        SFXManager.instance.PlayButtonClick();
        startPanel.SetActive(false);
        Time.timeScale = 1;

        FindObjectOfType<MusicManager>().PlayMusic(); // ?? включаем музыку
    }
}