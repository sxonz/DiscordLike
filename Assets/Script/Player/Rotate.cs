using UnityEngine;
using Photon.Pun;
public class Rotate : MonoBehaviourPun { 
    private Camera camera; 
    void Start() { 
        camera = Camera.main; 
    }
        void Update() {
        if (!photonView.IsMine) return;
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirVec = mousePos - (Vector2)transform.position; transform.up = dirVec.normalized; 
    } 
}