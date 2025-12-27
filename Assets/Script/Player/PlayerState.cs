using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class PlayerState : MonoBehaviourPun
{
    private PlayerAnima playerAnima;

    public float PLAYER_MAX_HP = 10f;
    private float playerHP;

    public float hit_delay = 1.0f;

    private bool isInvincible = false;
    private Tween invincibleTween;

    void Start()
    {
        playerAnima = GetComponent<PlayerAnima>();
        playerHP = PLAYER_MAX_HP;
    }

    [PunRPC]
    public void RPC_Hit(float damage)
    {
        // 연출은 전원
        playerAnima.PlayHitEffect(hit_delay);

        // 판정은 오너만
        if (!photonView.IsMine)
            return;

        if (isInvincible)
            return;

        isInvincible = true;

        invincibleTween?.Kill();
        invincibleTween = DOVirtual.DelayedCall(hit_delay, () =>
        {
            isInvincible = false;
        });

        playerHP -= damage;

        if (playerHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    void OnDestroy()
    {
        invincibleTween?.Kill();
    }
}
