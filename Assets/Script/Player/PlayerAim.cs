using Photon.Pun;
using UnityEngine;

public class PlayerAim : MonoBehaviourPun
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        transform.up = dir;
    }
}