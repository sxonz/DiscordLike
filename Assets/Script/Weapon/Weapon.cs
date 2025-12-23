using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float attackAngle = 90f;     // 휘두르는 각도
    public float attackDuration = 0.15f; // 공격 시간

    bool isAttacking = false;

    public void Attack(bool isLeftHand)
    {
        if (isAttacking) return;
        StartCoroutine(AttackCoroutine(isLeftHand));
    }

    IEnumerator AttackCoroutine(bool isLeftHand)
    {
        isAttacking = true;

        float elapsed = 0f;
        float direction = isLeftHand ? 1f : -1f; // 왼손 / 오른손 방향 반전

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

        // 원래 각도로 복귀
        transform.localRotation = startRot;
        isAttacking = false;
    }
}