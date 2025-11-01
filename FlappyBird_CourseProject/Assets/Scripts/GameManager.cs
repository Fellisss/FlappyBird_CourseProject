using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public int lives = 3; // ?? количество жизней
    public TextMeshProUGUI livesText; // ссылка на текст для отображения жизней
    public GameObject gameOverPanel; // панель с сообщением о проигрыше
    public GameObject winPanel;
    public AudioClip winSound;
    private AudioSource audioSource;

    public TextMeshProUGUI bestScoreText; // Текст для отображения лучшего результата
    private int bestScore = 0; // Лучший результат

    void Start()
    {
        Time.timeScale = 1;
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // Загружаем лучший результат
        UpdateBestScoreText();
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void UpdateBestScoreText()
    {
        if (bestScoreText != null)
            bestScoreText.text = "Лучший результат: " + bestScore;
    }

    public void WinGame()
    {
        Time.timeScale = 0; // Останавливаем игру
        FindObjectOfType<MusicManager>().StopMusic();

        if (winPanel != null)
            winPanel.SetActive(true);

        if (winSound != null)
            audioSource.PlayOneShot(winSound);
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();

        if (score >= 50) // ? Победа при 30 очках
        {
            WinGame();
        }
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();

        if (livesText != null)
            livesText.text = "" + lives.ToString();
    }

    public void GameOver()
    {
        SFXManager.instance.PlayButtonClick();
        // Останавливаем время
        Time.timeScale = 0;

        SFXManager.instance.PlayLoseLife();
        // Останавливаем музыку
        FindObjectOfType<MusicManager>().StopMusic();

        // Сохраняем лучший результат
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        Time.timeScale = 0;
        FindObjectOfType<MusicManager>().StopMusic();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    //public void RestartGame()
    //{
    //    Time.timeScale = 1;
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}