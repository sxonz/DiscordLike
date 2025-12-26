using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float PLAYER_MAX_HP = 10;
    float playerHP;

    void Start()
    {
        playerHP = PLAYER_MAX_HP;
    }

    public void Hit(float damage)
    {
        playerHP -= damage;
        if (playerHP <= 0)
        {
            Debug.Log("플레이어 사망");
            die();
        }
    }
    void die()
    {
        Destroy(gameObject);
    }
}
