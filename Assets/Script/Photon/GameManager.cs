using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Screen.SetResolution(1600, 900, false);

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-4f, 4f),
            Random.Range(-2f, 2f),
            0
        );

        PhotonNetwork.Instantiate(
            "Player", 
            spawnPos,
            Quaternion.identity
        );
    }
}
