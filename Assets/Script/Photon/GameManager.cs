using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public TextMeshProUGUI winnerTXT;

    private List<Player> alivePlayers = new List<Player>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Screen.SetResolution(1600, 900, false);
    }

    void Start()
    {
        SpawnPlayer();

        // 방에 있는 플레이어 모두 등록
        foreach (var p in PhotonNetwork.PlayerList)
        {
            alivePlayers.Add(p);
        }
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

    // 플레이어가 탈락했을 때 호출
    public void OnPlayerEliminated(Player eliminatedPlayer)
    {
        // 리스트에서 제거
        alivePlayers.Remove(eliminatedPlayer);

        // 남은 플레이어가 1명일 때 승리 처리
        if (alivePlayers.Count == 1)
        {
            Player winner = alivePlayers[0];
            winnerTXT.text = winner.NickName + " 승리!";
        }
    }
}
