using Photon.Pun;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    bool isStuck = false;

    public float damage = 2;

    public int ownerActorNumber; // 화살을 쏜 플레이어 ActorNumber

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStuck) return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            StickAndDisappear();
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView playerPV = collision.gameObject.GetComponent<PhotonView>();
            if (playerPV == null) return;

            // 자기 화살은 자신에게 적용하지 않음
            if (playerPV.OwnerActorNr == ownerActorNumber)
                return;

            // 맞은 플레이어에게 RPC 호출
            playerPV.RPC("RPC_Hit", RpcTarget.All, damage);

            StickAndDisappear();
        }
    }

    void StickAndDisappear()
    {
        isStuck = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        GetComponent<Collider2D>().enabled = false;

        DOVirtual.DelayedCall(1.5f, () =>
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Destroy(gameObject);
            else
                Destroy(gameObject);
        });
    }

    void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}
