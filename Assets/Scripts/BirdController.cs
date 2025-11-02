using UnityEngine;

public class Bird : MonoBehaviour
{
    public float jumpForce = 5f;
    private Rigidbody2D rb;

    public AudioClip hitSound; // звук столкновения
    private AudioSource audioSource; // источник звука

    private float tiltSmooth = 5f;      // скорость плавного поворота
    private float maxUpRotation = 15f;  // угол наклона вверх (уменьшен)
    private float maxDownRotation = -25f; // угол наклона вниз (уменьшен)
    private float targetRotation = 0f;  // текущая цель поворота

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // === Управление полётом ===
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // Обнуляем вертикальную скорость и добавляем импульс вверх
            rb.velocity = Vector2.up * jumpForce;
            SFXManager.instance.PlayJump();

            // При взмахе — слегка поднимаем нос вверх
            targetRotation = maxUpRotation;
        }

        // === Постепенный наклон вниз при падении ===
        if (rb.velocity.y < 0)
        {
            targetRotation = maxDownRotation;
        }

        // === Плавно интерполируем поворот ===
        float currentZ = transform.rotation.eulerAngles.z;
        if (currentZ > 180) currentZ -= 360; // исправляем угол
        float newZ = Mathf.Lerp(currentZ, targetRotation, tiltSmooth * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newZ);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // если столкновение с трубой / метеоритом
        if (collision.gameObject.CompareTag("Pipe"))
        {
            audioSource.PlayOneShot(hitSound);
            FindObjectOfType<GameManager>().LoseLife();
        }
    }
}