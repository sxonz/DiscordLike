using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    public State state;
    void Start()
    {
        state = GetComponent<State>();
    }

    protected bool isAttacking = false;

    // 
    public virtual void SpecialAttack()
    {
        Debug.Log($"{name} Ư�� ���� (�⺻)");
    }

    public virtual bool isbow()
    {
        return false;
    }
}