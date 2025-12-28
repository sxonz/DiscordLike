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

    void OnCollisionEnter2D(Collision2D collision)
    {
        PhotonView arrowPV = GetComponent<PhotonView>();

        // 판정은 화살 오너만
        if (!arrowPV.IsMine)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView playerPV =
                collision.gameObject.GetComponent<PhotonView>();
            if (playerPV == null) return;

            // 자기 자신 무시
            if (playerPV.OwnerActorNr == arrowPV.OwnerActorNr)
                return;

            playerPV.RPC("RPC_Hit", RpcTarget.All, damage);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void OnDisable()
    {
        attackTween?.Kill();
        if (axeCollider != null)
            axeCollider.enabled = false;

        isAttacking = false;
    }
}
