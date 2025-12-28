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

    [Header("스폰 포인트 배열 (Vector3)")]
    public Vector3[] spawnPositions; // 인스펙터에서 위치 지정

    private List<Player> alivePlayers = new List<Player>();
    private static List<Vector3> availableSpawnPositions = new List<Vector3>();
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
        // 사용 가능한 스폰 위치 초기화
        availableSpawnPositions = new List<Vector3>(spawnPositions);

        SpawnPlayer();

        foreach (var p in PhotonNetwork.PlayerList)
            alivePlayers.Add(p);
    }

    void SpawnPlayer()
    {
        if (availableSpawnPositions.Count == 0)
        {
            Debug.LogError("모든 스폰 포인트가 사용되었습니다!");
            return;
        }

        // 랜덤한 위치 선택
        int index = Random.Range(0, availableSpawnPositions.Count);
        Vector3 spawnPos = availableSpawnPositions[index];

        // 선택한 위치 제거
        availableSpawnPositions.RemoveAt(index);

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
                    MoveToChat();
                }).SetUpdate(true);
            }
        }
    }

    [PunRPC]
    void MoveToChat()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.LoadLevel(1);
    }
}
