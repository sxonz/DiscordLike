using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

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

        // ★ 핵심: 무기 소유권을 이 플레이어에게 이전
        if (!weaponPV.IsMine)
        {
            weaponPV.TransferOwnership(photonView.Owner);
        }

        Transform targetHand;
        Vector3 scale = new Vector3(2, 2, 2);

        if (leftWeapon == null)
        {
            leftWeapon = weapon;
            targetHand = leftHand;
            state.isLeftHand = true;
            state.isRightHand = false;
        }
        else if (rightWeapon == null)
        {
            rightWeapon = weapon;
            targetHand = rightHand;
            state.isLeftHand = false;
            state.isRightHand = true;
            scale = new Vector3(-2, 2, 2);
        }
        else
        {
            return false;
        }

        PhotonView handPV = targetHand.GetComponent<PhotonView>();
        if (handPV == null)
            return false;

        photonView.RPC(
            "PickWeapon",
            RpcTarget.All,
            weaponPV.ViewID,
            handPV.ViewID,
            0f,
            scale
        );

        state.isDropped = false;
        return true;
    }


    // 부모 동기화 전용 RPC
    [PunRPC]
    void PickWeapon(int weaponViewID, int handViewID, float angle, Vector3 scale)
    {
        PhotonView weaponPV = PhotonView.Find(weaponViewID);
        PhotonView handPV = PhotonView.Find(handViewID);

        if (weaponPV == null || handPV == null)
            return;

        Transform weaponTr = weaponPV.transform;
        Transform handTr = handPV.transform;

        weaponTr.SetParent(handTr, false);
        weaponTr.localPosition = Vector3.zero;
        weaponTr.localRotation = Quaternion.Euler(0, 0, angle);
        weaponTr.localScale = scale;
    }
}
