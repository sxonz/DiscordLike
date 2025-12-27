using UnityEngine;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
{
    public float PLAYER_MAX_HP = 10f;
    private float playerHP;

    void Start()
    {
        playerHP = PLAYER_MAX_HP;
    }

    // 외부에서 호출하는 진입점 (무기에서 호출)
    [PunRPC]
    public void RPC_Hit(float damage)
    {
        // 체력 계산은 반드시 오너만
        if (!photonView.IsMine)
            return;

        playerHP -= damage;

        if (playerHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");

        // 오너만 네트워크 오브젝트 삭제
        PhotonNetwork.Destroy(gameObject);
    }
}
