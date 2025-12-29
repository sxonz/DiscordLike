using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class Arrow : MonoBehaviourPun
{
    Rigidbody2D rb;
    bool isStuck = false;

    public float damage = 2f;

    PhotonView arrowPV; // [추가] PhotonView 캐싱

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        arrowPV = GetComponent<PhotonView>(); // [추가]
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 판정은 화살 오너만
        if (!arrowPV.IsMine)
            return;

        if (isStuck)
            return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            arrowPV.RPC("RPC_StickAndDisappear", RpcTarget.All); // [수정] RPC로 전파
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
        
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    // [추가] 모든 클라이언트에서 실행
    [PunRPC]
    void RPC_StickAndDisappear()
    {
        if (isStuck)
            return;

        isStuck = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        GetComponent<Collider2D>().enabled = false;

        DOVirtual.DelayedCall(1.5f, () =>
        {
            if (PhotonNetwork.IsConnected && arrowPV.IsMine)
                PhotonNetwork.Destroy(gameObject); // [중요] 오너만 Destroy
        });
    }

    void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}
