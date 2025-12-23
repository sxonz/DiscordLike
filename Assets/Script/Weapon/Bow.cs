using UnityEngine;

public class Bow : Weapon
{
    [Header("Arrow")]
    public GameObject arrowPrefab;   // 화살 프리팹
    public Transform firePoint;       // 발사 위치
    public float arrowSpeed = 12f;

    public override void SpecialAttack()
    {
        if (arrowPrefab == null || firePoint == null)
        {
            Debug.LogWarning("ArrowPrefab or FirePoint missing!");
            return;
        }

        GameObject arrow = Instantiate(
            arrowPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.up * arrowSpeed;
        }
    }
}