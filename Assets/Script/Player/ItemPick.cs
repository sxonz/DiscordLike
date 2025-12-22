using UnityEngine;

public class ItemPick : MonoBehaviour
{
    public Transform rightHand;   // 무기 붙일 손
    private GameObject currentWeapon;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!Input.GetKeyDown(KeyCode.F))
            return;

        if (!other.CompareTag("weapon"))
            return;

        State state = other.GetComponent<State>();
        if (state == null || !state.isDropped)
            return;

        PickWeapon(other.gameObject);
    }

    void PickWeapon(GameObject weapon)
    {
        // 이미 무기 들고 있으면 무시 (또는 교체 로직 가능)
        if (currentWeapon != null)
            return;

        State state = weapon.GetComponent<State>();
        state.isDropped = false;

        // 손에 붙이기
        weapon.transform.SetParent(rightHand);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        // 충돌 비활성화
        Collider2D col = weapon.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // 물리 제거 (있다면)
        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.simulated = false;

        currentWeapon = weapon;

        Debug.Log("무기 장착 완료");
    }
}