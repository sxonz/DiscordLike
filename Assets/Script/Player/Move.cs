using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Move : MonoBehaviourPun
{
    public float moveSpeed = 3f;

    [Header("Dash")]
    public float dashMultiplier = 3f;   // ��� �� �ӵ� ���
    public float dashDuration = 0.5f;   // ��� ���� �ð�
    public float dashCooldown = 3f;     // ��Ÿ��

    bool isDashing = false;
    bool canDash = true;

    void Update()
    {
        if (!photonView.IsMine) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized;

        float speed = isDashing ? moveSpeed * dashMultiplier : moveSpeed;
        transform.Translate(movement * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
