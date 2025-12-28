using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class Axe : Weapon
{
    public float attackAngle = 90f;
    public float attackDuration = 0.15f;
    public float damage = 3f;

    private Tween attackTween;
    private Collider2D axeCollider;

    private bool hasHit; // 추가: 한 번의 공격에서 중복 히트 방지

    void Awake()
    {
        axeCollider = GetComponent<Collider2D>();
    }

    public override void SpecialAttack()
    {
        if (isAttacking)
            return;

        Attack(state.isLeftHand);
    }

    private void Attack(bool isLeftHand)
    {
        isAttacking = true;
        hasHit = false;               // 추가: 공격 시작 시 히트 초기화
        axeCollider.enabled = true;

        float direction = isLeftHand ? 1f : -1f;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot =
            startRot * Quaternion.Euler(0f, 0f, direction * attackAngle);

        attackTween?.Kill();

        attackTween = transform
            .DOLocalRotateQuaternion(endRot, attackDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.localRotation = startRot;
                axeCollider.enabled = false;
                isAttacking = false;
            });
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttacking) return;     // 추가: 공격 중이 아니면 무시
        if (hasHit) return;           // 추가: 이미 맞췄으면 무시

        PhotonView axePV = GetComponent<PhotonView>(); // 변수명 의미 맞게 사용

        // 판정은 도끼 오너만
        if (!axePV.IsMine)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView playerPV =
                collision.gameObject.GetComponent<PhotonView>();
            if (playerPV == null) return;

            // 자기 자신 무시
            if (playerPV.OwnerActorNr == axePV.OwnerActorNr)
                return;

            hasHit = true; // 추가: 한 번 맞췄다고 기록
            playerPV.RPC("RPC_Hit", RpcTarget.All, damage);

            // 수정: 도끼는 파괴하지 않음
        }
    }

    void OnDisable()
    {
        attackTween?.Kill();
        if (axeCollider != null)
            axeCollider.enabled = false;

        isAttacking = false;
        hasHit = false; // 추가
    }
}
