using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    bool isStuck = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStuck) return;

        // 🔹 벽에 닿으면 꽃힘
        if (collision.gameObject.CompareTag("Wall"))
        {
            Stick();
        }

        // 🔹 플레이어 맞으면 (일단 로그)
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 맞음");
            Stick();
        }
    }

    void Stick()
    {
        isStuck = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        GetComponent<Collider2D>().enabled = false;
    }
}