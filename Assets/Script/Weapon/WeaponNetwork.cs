using Photon.Pun;
using UnityEngine;

public class WeaponNetwork : MonoBehaviourPun
{
    [PunRPC]
    public void RPC_SetPicked(bool picked)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = !picked;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = !picked;
    }
}