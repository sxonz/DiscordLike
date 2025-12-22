using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(0, 0, angle - 90f);

        leftHand.rotation = rot;
        rightHand.rotation = rot;
    }
}
