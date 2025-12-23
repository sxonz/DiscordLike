using UnityEngine;
using Photon.Pun;

public class Bow : Weapon
{
    public string arrowPrefabName = "Arrow"; 
    public Transform firePoint;
    public float arrowSpeed = 12f;

    public override void SpecialAttack()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        GameObject arrow = PhotonNetwork.Instantiate(
            arrowPrefabName,
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