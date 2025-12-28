using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public TextMeshProUGUI winnerTXT;
    private List<Player> alivePlayers = new List<Player>();

    [SerializeField] private float moveToChatDelay = 3f;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Screen.SetResolution(1600, 900, false);
    }

    void Start()
    {
        SpawnPlayer();

        foreach (var p in PhotonNetwork.PlayerList)
            alivePlayers.Add(p);
    }

    void SpawnPlayer()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-4f, 4f),
            Random.Range(-2f, 2f),
            0
        );

        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }

    public void OnPlayerEliminated(Player eliminatedPlayer)
    {
        if (gameEnded) return;

        alivePlayers.Remove(eliminatedPlayer);

        if (alivePlayers.Count == 1)
        {
            gameEnded = true;

            Player winner = alivePlayers[0];
            winnerTXT.text = winner.NickName + " 승리!";

            if (PhotonNetwork.IsMasterClient)
            {
                DOVirtual.DelayedCall(moveToChatDelay, () =>
                {
                    MoveToChat(); // RPC 말고 직접 호출
                }).SetUpdate(true);
            }
        }
    }

    // RPC 유지하지만 실제로는 마스터만 LoadLevel
    [PunRPC]
    void MoveToChat()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.LoadLevel(1);
    }
}
