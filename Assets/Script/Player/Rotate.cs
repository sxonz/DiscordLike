using UnityEngine;
public class Rotate : MonoBehaviour { 
    private Camera camera; 
    void Start() { 
        camera = Camera.main; 
    }
        void Update() {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirVec = mousePos - (Vector2)transform.position; transform.up = dirVec.normalized; 
    } 
}