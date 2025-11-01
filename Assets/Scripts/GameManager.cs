using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public int lives = 3;
    public GameObject[] hearts; // массив UI-сердечек

    public GameObject gameOverPanel;
    public GameObject winPanel;
    public AudioClip winSound;
    private AudioSource audioSource;

    public TextMeshProUGUI bestScoreText;
    private int bestScore = 0;

    [Header("?? Новые элементы интерфейса")]
    public GameObject startPanel; // Панель с кнопкой "Играть"
    public GameObject pipeSpawner; // Объект спавнера метеоритов
    public GameObject pauseButton; // Кнопка паузы

    private bool gameStarted = false; // Проверка, началась ли игра

    void Start()
    {
        Time.timeScale = 0; // ?? Игра заморожена на старте
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateBestScoreText();

        audioSource = GetComponent<AudioSource>();

        // Скрываем все UI сердца на старте
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }

        if (startPanel != null) startPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (pipeSpawner != null) pipeSpawner.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(false);
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1;

        if (startPanel != null) startPanel.SetActive(false);
        if (pipeSpawner != null) pipeSpawner.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(true);

        // Показываем сердца на старте игры
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(true);
            heart.transform.localScale = Vector3.one; // сброс масштаба
        }

        UpdateUI();
        score = 0;
        lives = hearts.Length;
    }

    void UpdateBestScoreText()
    {
        if (bestScoreText != null)
            bestScoreText.text = "Лучший результат: " + bestScore;
    }

    public void AddScore()
    {
        if (!gameStarted) return;

        score++;
        scoreText.text = score.ToString();

        if (score >= 30) // Победа
        {
            WinGame();
        }
    }

    public void LoseLife()
    {
        if (!gameStarted) return;

        if (lives <= 0) return;

        // Плавно скрываем сердце
        StartCoroutine(HideHeart(hearts[lives - 1]));

        lives--;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    IEnumerator HideHeart(GameObject heart)
    {
        Vector3 startScale = heart.transform.localScale;
        float duration = 0.3f; // длительность анимации
        float elapsed = 0f;

        while (elapsed < duration)
        {
            heart.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        heart.transform.localScale = Vector3.zero;
        heart.SetActive(false);
    }

    public void GameOver()
    {
        gameStarted = false;

        if (pauseButton != null)
            pauseButton.SetActive(false);

        SFXManager.instance.PlayButtonClick();
        Time.timeScale = 0;
        SFXManager.instance.PlayLoseLife();
        FindObjectOfType<MusicManager>().StopMusic();

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Скрываем все сердца при GameOver
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }
    }

    public void WinGame()
    {
        gameStarted = false;
        Time.timeScale = 0;
        FindObjectOfType<MusicManager>().StopMusic();
        if (winPanel != null) winPanel.SetActive(true);

        if (winSound != null)
            audioSource.PlayOneShot(winSound);

        // Скрываем все сердца
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        // На всякий случай обновляем все сердца, если нужно
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < lives && gameStarted);
        }
    }
}