using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    public State state;

    protected int ownerActorNumber;

    protected bool isAttacking = false;

    void Start()
    {
        state = GetComponent<State>();

        // 무기를 들고 있는 플레이어의 ActorNumber
        ownerActorNumber = photonView.OwnerActorNr;
    }

    public virtual void SpecialAttack()
    {
        Debug.Log($"{name} 기본 공격");
    }

    public virtual bool isbow()
    {
        return false;
    }
}
