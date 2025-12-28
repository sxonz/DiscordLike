using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class PlayerState : MonoBehaviourPun
{
    private PlayerAnima playerAnima;

    private WeaponHolder weaponHolder;

    public float PLAYER_MAX_HP = 10f;
    private float playerHP;

    public float hit_delay = 1.0f;

    private bool isInvincible = false;
    private Tween invincibleTween;

    void Start()
    {
        weaponHolder = GetComponent<WeaponHolder>();

        playerAnima = GetComponent<PlayerAnima>();
        playerHP = PLAYER_MAX_HP;
    }

    [PunRPC]
    public void RPC_Hit(float damage)
    {
        // 1. HP 감소 먼저
        playerHP -= damage;

        if (playerHP <= 0f)
        {
            Die();
            return;
        }

        // 2. 무적 처리
        if (isInvincible)
            return;

        playerAnima.PlayHitEffect(hit_delay);

        isInvincible = true;

        invincibleTween?.Kill();
        invincibleTween = DOVirtual.DelayedCall(hit_delay, () =>
        {
            isInvincible = false;
        });
    }

    void Die()
    {
        GameManager.Instance.OnPlayerEliminated(photonView.Owner);
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            weaponHolder.die();
        }
    }
    void OnDestroy()
    {
        invincibleTween?.Kill();
    }
}
