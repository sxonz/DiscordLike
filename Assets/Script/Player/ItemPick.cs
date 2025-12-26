using UnityEngine;
using Photon.Pun;

public class ItemPick : MonoBehaviourPun
{
    public WeaponHolder weaponHolder;

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
        if (!photonView.IsMine) return;

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
        weaponHolder.EquipWeapon(weapon);

        WeaponNetwork wN = weapon.GetComponent<WeaponNetwork>();
        if (wN != null)
        {
            wN.photonView.RPC(
                "RPC_SetPicked",
                RpcTarget.All,
                true
            );
        }
    }

}