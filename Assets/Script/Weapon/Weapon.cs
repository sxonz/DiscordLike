using UnityEngine;

public class Weapon : MonoBehaviour
{
    public void Attack()
    {
        Debug.Log($"{name} 공격!");
        // 나중에 여기서 실제 공격 로직 작성
    }
}