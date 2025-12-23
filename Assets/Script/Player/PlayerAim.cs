using Photon.Pun;
using UnityEngine;

public class PlayerAim : MonoBehaviourPun
{
    Camera cam;
    public float angleOffset = -90f;

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