using Photon.Pun;
using UnityEngine;
using DG.Tweening; // [추가] DOTween 사용

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    bool isStuck = false;

    public float damage = 2;

    PhotonView pv; // [추가] 오너 체크용

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>(); // [추가]
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStuck) return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("화살이 벽에 꽃힘");
            StickAndDisappear();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 맞음");
            OnPlayerHit(collision.gameObject);
        }
    }

    void StickAndDisappear()
    {
        isStuck = true;

        // 이동 정지
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        // 충돌 끄기
        GetComponent<Collider2D>().enabled = false;

        // 1.5초 후 사라짐 (DOTween 사용)
        Debug.Log("화살 1.5초 후 제거");

        if (!pv.IsMine) return; // [추가] 오너만 삭제

        DOVirtual.DelayedCall(1.5f, () => // [추가]
        {
            PhotonNetwork.Destroy(gameObject); // [수정] 지연 삭제
        });
    }

    void OnPlayerHit(GameObject player)
    {
        PlayerState playerState = player.GetComponent<PlayerState>();
        playerState.Hit(damage);

        Debug.Log("화살 바로 제거");

        if (!pv.IsMine) return; // [추가] 오너만 삭제
        PhotonNetwork.Destroy(gameObject);
    }

    void OnDestroy()
    {
        DOTween.Kill(gameObject); // [추가] 트윈 정리 (에러 방지)
    }
}
