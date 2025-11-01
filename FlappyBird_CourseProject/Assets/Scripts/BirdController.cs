using UnityEngine;

public class Bird : MonoBehaviour
{
    public float jumpForce = 5f;
    private Rigidbody2D rb;

    public AudioClip hitSound; // ?? звук столкновения
    private AudioSource audioSource; // источник звука

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // Если нажали пробел или кликнули мышкой
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // обнуляем вертикальную скорость и добавляем импульс вверх
            rb.velocity = Vector2.up * jumpForce;
            SFXManager.instance.PlayJump();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        // если столкновение с трубой
        if (collision.gameObject.CompareTag("Pipe"))
        {
            audioSource.PlayOneShot(hitSound);
            FindObjectOfType<GameManager>().LoseLife();
        }
    }
}
