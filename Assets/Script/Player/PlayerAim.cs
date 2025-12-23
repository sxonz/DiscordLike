using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        transform.up = dir;
    }
}