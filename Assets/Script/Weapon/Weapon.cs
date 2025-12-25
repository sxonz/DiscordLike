using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    public float attackAngle = 90f;
    public float attackDuration = 0.15f;

    protected bool isAttacking = false;

    // 
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

    // 
    public virtual void SpecialAttack()
    {
        Debug.Log($"{name} Ư�� ���� (�⺻)");
    }

    // 무기 집기 / 버리기 상태 동기화
    [PunRPC]
    public void RPC_SetPicked(bool picked)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = !picked;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = !picked;
    }
}