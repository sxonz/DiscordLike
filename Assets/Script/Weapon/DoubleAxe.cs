using DG.Tweening;
using UnityEngine;

public class DoubleAxe : Weapon
{
    public float attackAngle = 90f;
    public float attackDuration = 0.15f;

    private Tween attackTween;

    public override void SpecialAttack()
    {
        if (isAttacking)
            return;

        Attack(state.isLeftHand);
    }

    private void Attack(bool isLeftHand)
    {
        isAttacking = true;

        float direction = isLeftHand ? 1f : -1f;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot =
            startRot * Quaternion.Euler(0f, 0f, direction * attackAngle);

        // 혹시 남아있는 트윈이 있다면 정리
        attackTween?.Kill();

        attackTween = transform
            .DOLocalRotateQuaternion(endRot, attackDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.localRotation = startRot;
                isAttacking = false;
            });
    }
}
