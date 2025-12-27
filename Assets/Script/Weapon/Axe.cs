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

    void Awake()
    {
        axeCollider = GetComponent<Collider2D>();
    }

    public override void SpecialAttack()
    {
        // 공격은 오너만 수행
        if (!photonView.IsMine)
            return;

        if (isAttacking)
            return;

        Attack(state.isLeftHand);
    }

    private void Attack(bool isLeftHand)
    {
        isAttacking = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        // 판정은 반드시 오너만
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

        // 데미지는 RPC로 전달
        targetPV.RPC("RPC_Hit", RpcTarget.All, damage);
    }

    void OnDisable()
    {
        attackTween?.Kill();
        if (axeCollider != null)
            axeCollider.enabled = false;

        isAttacking = false;
    }
}
