using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        // движение трубы влево
        transform.position += Vector3.left * speed * Time.deltaTime;

        // если труба ушла далеко влево Ч уничтожаем еЄ
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}