using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class Bow : Weapon
{
    public string arrowPrefabName = "Arrow";
    public Transform firePoint;
    public float arrowSpeed = 12f;

    public float shootingDelay = 3f;
    private bool isCooldown = false;

    public override void SpecialAttack()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (isCooldown) return;

        Shoot();
        StartCooldown();
    }

    void Shoot()
    {
        GameObject arrow = PhotonNetwork.Instantiate(
            arrowPrefabName,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = firePoint.up * arrowSpeed;

        // 핵심: 자기 플레이어와 충돌 무시
        Collider2D arrowCol = arrow.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponentInParent<Collider2D>();

        if (arrowCol != null && playerCol != null)
            Physics2D.IgnoreCollision(arrowCol, playerCol);
    }

    void StartCooldown()
    {
        isCooldown = true;
        DOVirtual.DelayedCall(shootingDelay, () => isCooldown = false);
    }

    public override bool isbow()
    {
        return true;
    }
}
