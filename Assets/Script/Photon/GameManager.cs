using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using DG.Tweening;
using UnityEngine.Tilemaps;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI winnerTXT;

    [Header("Spawn Positions")]
    [SerializeField] private Vector3[] spawnPositions;

    [Header("Spawn Tilemap Fade")]
    [SerializeField] private Tilemap spawnTilemap;
    [SerializeField] private float tilemapFadeDuration = 2f;

    [Header("Game Flow")]
    [SerializeField] private float moveToChatDelay = 3f;

    private List<Player> alivePlayers = new List<Player>();
    private bool gameEnded = false;

    private const string SPAWN_SEED_KEY = "SpawnSeed";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        Screen.SetResolution(1600, 900, false);
    }

    void Start()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < spawnPositions.Length; i++)
            availableIndices.Add(i);

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // 랜덤으로 SpawnPosition 선택
            int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
            availableIndices.Remove(randomIndex);

            Vector3 spawnPos = spawnPositions[randomIndex];

            // 각 플레이어가 자기 자신만 Instantiate
            if (p == PhotonNetwork.LocalPlayer)
                PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        }

        FadeOutSpawnTilemap();

        alivePlayers.Clear();
        foreach (Player p in PhotonNetwork.PlayerList)
            alivePlayers.Add(p);
    }


    // =========================
    // Spawn Logic (고쳐진 핵심)
    // =========================

    void SpawnPlayer()
    {
        if (spawnPositions == null || spawnPositions.Length == 0)
        {
            Debug.LogError("SpawnPositions 비어있음");
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount > spawnPositions.Length)
        {
            Debug.LogError("플레이어 수가 스폰 포인트보다 많음");
            return;
        }

        // ⭐ 핵심: ActorNumber 기반 "랜덤처럼 보이는" 분산
        int index = PhotonNetwork.LocalPlayer.ActorNumber % spawnPositions.Length;
        Vector3 spawnPos = spawnPositions[index];

        PhotonNetwork.Instantiate(
            "Player",
            spawnPos,
            Quaternion.identity
        );
    }


    // =========================
    // Tilemap Fade
    // =========================

    void FadeOutSpawnTilemap()
    {
        if (spawnTilemap == null)
            return;

        Color startColor = spawnTilemap.color;
        startColor.a = 1f;
        spawnTilemap.color = startColor;

        DOTween.To(
            () => spawnTilemap.color,
            c => spawnTilemap.color = c,
            new Color(startColor.r, startColor.g, startColor.b, 0f),
            tilemapFadeDuration
        ).SetEase(Ease.OutQuad);
    }

    // =========================
    // Game End Logic
    // =========================

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
                DOVirtual.DelayedCall(moveToChatDelay, MoveToChat)
                         .SetUpdate(true);
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
