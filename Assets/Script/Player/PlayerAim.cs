using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Camera cam;
    public Transform leftHand;
    public Transform rightHand;

    void Start()
    {
        cam = Camera.main;
        transform.position = (leftHand.position + rightHand.position) / 2f;

    }

    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // offset¸¸ È¸Àü
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
