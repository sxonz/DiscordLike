using UnityEngine;
using Photon.Pun;
using Unity.Cinemachine;

public class CameraBinder : MonoBehaviourPun
{
    void OnEnable()
    {
        if (!photonView.IsMine) return;

        var cam = Object.FindAnyObjectByType<CinemachineCamera>();
        if (cam == null)
        {
            Debug.LogError("CinemachineCamera not found");
            return;
        }

        cam.Follow = transform;
        cam.LookAt = transform;
    }
}
