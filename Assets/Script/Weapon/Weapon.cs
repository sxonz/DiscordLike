using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float attackAngle = 90f;
    public float attackDuration = 0.15f;

    protected bool isAttacking = false;

    // 기본 공격 (근접 회전)
    public virtual void Attack(bool isLeftHand)
    {
        if (isAttacking) return;
        StartCoroutine(AttackCoroutine(isLeftHand));
    }

    protected IEnumerator AttackCoroutine(bool isLeftHand)
    {
        isAttacking = true;

        float elapsed = 0f;
        float direction = isLeftHand ? 1f : -1f;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot =
            startRot * Quaternion.Euler(0, 0, direction * attackAngle);

        while (elapsed < attackDuration)
        {
            transform.localRotation = Quaternion.Slerp(
                startRot,
                endRot,
                elapsed / attackDuration
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = startRot;
        isAttacking = false;
    }

    // 특수 공격 (무기마다 다르게)
    public virtual void SpecialAttack()
    {
        Debug.Log($"{name} 특수 공격 (기본)");
    }
}