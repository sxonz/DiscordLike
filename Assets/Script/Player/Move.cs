using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float moveSpeed = 3f;

    [Header("Dash")]
    public float dashMultiplier = 3f;   // 대시 중 속도 배수
    public float dashDuration = 0.5f;   // 대시 지속 시간
    public float dashCooldown = 3f;     // 쿨타임

    bool isDashing = false;
    bool canDash = true;

    void Update()
    {
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
