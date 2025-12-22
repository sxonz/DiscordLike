using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    private Weapon leftWeapon;
    private Weapon rightWeapon;

    // 외부(ItemPick)에서 호출
    public bool EquipWeapon(GameObject weaponObj)
    {
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        State state = weaponObj.GetComponent<State>();

        if (weapon == null || state == null)
            return false;

        if (leftWeapon == null)
        {
            AttachWeapon(weaponObj, leftHand);
            leftWeapon = weapon;
        }
        else if (rightWeapon == null)
        {
            AttachWeapon(weaponObj, rightHand);
            rightWeapon = weapon;
        }
        else
        {
            // 두 손 다 차있음
            return false;
        }

        state.isDropped = false;
        return true;
    }

    void AttachWeapon(GameObject weapon, Transform hand)
    {
        weapon.transform.SetParent(hand);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        Collider2D col = weapon.GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;
    }

    void Update()
    {
        // 좌클릭 → 왼손
        if (Input.GetMouseButtonDown(0) && leftWeapon != null)
        {
            leftWeapon.Attack();
        }

        // 우클릭 → 오른손
        if (Input.GetMouseButtonDown(1) && rightWeapon != null)
        {
            rightWeapon.Attack();
        }
    }
}
