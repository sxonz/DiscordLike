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

    // 공격 기준 회전 (절대 변하지 않음)
    private Quaternion baseLocalRotation;

    void Awake()
    {
        axeCollider = GetComponent<Collider2D>();

        // 바닥에 있을 때 기준 회전 저장
        baseLocalRotation = transform.localRotation;

        // 처음에는 바닥에 있으므로 콜라이더 켜둠
        axeCollider.enabled = true;
    }

    public override void SpecialAttack()
    {
        // 입력 차단 (로컬)
        if (isAttacking)
            return;

        // 공격 시작을 모든 클라이언트에 동기화
        photonView.RPC(
            nameof(RPC_Attack),
            RpcTarget.All,
            state.isLeftHand
        );
    }

    [PunRPC]
    void RPC_Attack(bool isLeftHand)
    {
        // RPC 수신 측에서도 반드시 차단
        if (isAttacking)
            return;

        Attack(isLeftHand);
    }

    private void Attack(bool isLeftHand)
    {
        isAttacking = true;
        axeCollider.enabled = true;

        float direction = isLeftHand ? 1f : -1f;

        Quaternion startRot = baseLocalRotation;
        Quaternion endRot =
            baseLocalRotation * Quaternion.Euler(0f, 0f, direction * attackAngle);

        // 누적 방지
        attackTween?.Kill();
        transform.localRotation = startRot;

        attackTween = transform
            .DOLocalRotateQuaternion(endRot, attackDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.localRotation = baseLocalRotation;
                axeCollider.enabled = false;
                isAttacking = false;
            });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 판정은 오너만 수행
        if (!photonView.IsMine)
            return;

        if (!isAttacking)
            return;

        if (!other.CompareTag("Player"))
            return;

        PhotonView targetPV = other.GetComponent<PhotonView>();
        if (targetPV == null)
            return;

        // 자기 자신 공격 방지
        if (targetPV.OwnerActorNr == photonView.OwnerActorNr)
            return;

        targetPV.RPC("RPC_Hit", RpcTarget.All, damage);
    }

    void OnDisable()
    {
        attackTween?.Kill();
        axeCollider.enabled = false;
        isAttacking = false;
    }
}
