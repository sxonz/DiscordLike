using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    bool isStuck = false;

    public float damage = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PhotonView arrowPV = GetComponent<PhotonView>();

        // 판정은 화살 오너만
        if (!arrowPV.IsMine)
            return;

        if (isStuck)
            return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            StickAndDisappear();
            return;
        }

        if (!collision.gameObject.CompareTag("Player"))
            return;

        PhotonView playerPV =
            collision.gameObject.GetComponentInParent<PhotonView>();

        if (playerPV == null)
            return;

        // 이제 자기 자신은 애초에 충돌 안 함 (IgnoreCollision)
        playerPV.RPC("RPC_Hit", RpcTarget.All, damage);
        PhotonNetwork.Destroy(gameObject);
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
