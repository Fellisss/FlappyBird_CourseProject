using System;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private bool passed = false; // очки прибавляются один раз

    void OnTriggerEnter2D(Collider2D other)
    {
        if (passed) return; // если уже срабатывал - выходим

        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().AddScore();
            Console.WriteLine("Очки"); // +1
            passed = true; // блокируем повторное срабатывание
        }
    }
}