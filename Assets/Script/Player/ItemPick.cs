using UnityEngine;

public class ItemPick : MonoBehaviour
{
    public Transform rightHand;

    Collider2D currentWeaponCol;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("weapon"))
        {
            currentWeaponCol = other;
            Debug.Log("Weapon in range");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == currentWeaponCol)
            currentWeaponCol = null;
    }

    void Update()
    {
        if (currentWeaponCol == null)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            State state = currentWeaponCol.GetComponent<State>();
            if (state != null && state.isDropped)
            {
                PickWeapon(currentWeaponCol.gameObject);
            }
        }
    }

    void PickWeapon(GameObject weapon)
    {
        Debug.Log("Pick Weapon!");

        State state = weapon.GetComponent<State>();
        state.isDropped = false;

        weapon.transform.SetParent(rightHand);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        Collider2D col = weapon.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;
    }
}