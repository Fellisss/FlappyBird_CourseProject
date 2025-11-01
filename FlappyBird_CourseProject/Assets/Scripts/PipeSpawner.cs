using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public float spawnX = 10f;
    public float gap = 5f;
    public float spawnRate = 2f;

    private float timer = 0f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnPipePair();
            timer = 0f;
        }
    }

    void SpawnPipePair()
    {
        // Получаем границы экрана в мировых координатах
        float topY = cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float bottomY = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        // Выбираем случайное смещение центра отверстия (не выходя за края)
        float centerY = Random.Range(bottomY + gap / 2f, topY - gap / 2f);

        // Нижняя труба (опирается на низ)
        GameObject bottomPipe = Instantiate(pipePrefab);
        bottomPipe.transform.position = new Vector3(spawnX, centerY - gap / 2f, 0);

        // Верхняя труба (опирается на верх)
        GameObject topPipe = Instantiate(pipePrefab);
        topPipe.transform.position = new Vector3(spawnX, centerY + gap / 2f, 0);
        topPipe.transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}