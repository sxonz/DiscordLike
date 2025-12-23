using UnityEngine;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 12f;

    public override void SpecialAttack()
    {
        if (arrowPrefab == null || firePoint == null)
            return;

        GameObject arrow = Instantiate(
            arrowPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.up * arrowSpeed;
        }
    }
}