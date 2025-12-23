using UnityEngine;
using Photon.Pun;

public class Rotate : MonoBehaviourPun
{
    Camera cam;
    public float angleOffset = -90f; // 90도 틀어지면 -90, 반대면 +90. 45면 -45 등으로 조절

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
    }
}