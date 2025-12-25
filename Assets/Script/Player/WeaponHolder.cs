using Photon.Pun;
using UnityEngine;

public class WeaponHolder : MonoBehaviourPun
{
    public Transform leftHand;
    public Transform rightHand;

    private Weapon leftWeapon;
    private Weapon rightWeapon;

    private PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        
        if (!photonView.IsMine) return;
        
        // 기존 로직 그대로 유지
        if (Input.GetMouseButtonDown(0) && leftWeapon != null)
        {
            leftWeapon.SpecialAttack();
        }

        if (Input.GetMouseButtonDown(1) && rightWeapon != null)
        {
            rightWeapon.SpecialAttack();
        }
    }

    public bool EquipWeapon(GameObject weaponObj)
    {
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        State state = weaponObj.GetComponent<State>();
        PhotonView weaponPV = weaponObj.GetComponent<PhotonView>();

        if (weapon == null || state == null || weaponPV == null)
            return false;

        Transform targetHand;

        if (leftWeapon == null)
        {
            leftWeapon = weapon;
            targetHand = leftHand;
        }
        else if (rightWeapon == null)
        {
            rightWeapon = weapon;
            targetHand = rightHand;
        }
        else
        {
            return false;
        }

        PhotonView handPV = targetHand.GetComponent<PhotonView>();
        if (handPV == null)
            return false;

        // 여기만 핵심 변경
        pv.RPC(
            "PickWeapon",
            RpcTarget.All,
            weaponPV.ViewID,
            handPV.ViewID
        );

        state.isDropped = false;
        return true;
    }

    // 부모 동기화 전용 RPC
    [PunRPC]
    void PickWeapon(int weaponViewID, int handViewID)
    {
        PhotonView weaponPV = PhotonView.Find(weaponViewID);
        PhotonView handPV = PhotonView.Find(handViewID);

        if (weaponPV == null || handPV == null)
            return;

        Transform weaponTr = weaponPV.transform;
        Transform handTr = handPV.transform;

        weaponTr.SetParent(handTr, false);
        weaponTr.localPosition = Vector3.zero;
        weaponTr.localRotation = Quaternion.Euler(0, 0, 90);
    }
}
