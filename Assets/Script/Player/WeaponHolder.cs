using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    private Weapon leftWeapon;
    private Weapon rightWeapon;

    void Update()
    {
        // 좌클릭 → 왼손 공격
        if (Input.GetMouseButtonDown(0) && leftWeapon != null)
        {
            leftWeapon.Attack(true);
        }

        // 우클릭 → 오른손 공격
        if (Input.GetMouseButtonDown(1) && rightWeapon != null)
        {
            rightWeapon.Attack(false);
        }
    }

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
            return false;
        }

        state.isDropped = false;
        return true;
    }
    void AttachWeapon(GameObject weapon, Transform hand)
    {
        weapon.transform.SetParent(hand, false);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        // scale 건들지 않음

        weapon.transform.localRotation = Quaternion.Euler(0, 0, -45);
    }
}