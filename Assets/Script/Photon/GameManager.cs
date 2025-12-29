using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public TextMeshProUGUI winnerTXT;

    [Header("스폰 포인트 배열 (Vector3)")]
    public Vector3[] spawnPositions; // 인스펙터에서 위치 지정

    [Header("타일맵")]
    public Tilemap tilemap;          // Inspector에서 할당
    public float fadeDuration = 2f;  // 투명화 시간

    private List<Player> alivePlayers = new List<Player>();
    [SerializeField] private float moveToChatDelay = 3f;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SpawnPlayer();

        foreach (var p in PhotonNetwork.PlayerList)
            alivePlayers.Add(p);


        FadeTilemap();
    }

    void SpawnPlayer()
    {
        // 랜덤한 위치 선택
        int index = Random.Range(0, spawnPositions.Length);
        Vector3 spawnPos = spawnPositions[index];

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
    void FadeTilemap()
    {
        if (tilemap == null) return;

        Color startColor = tilemap.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // DOTween.To 사용
        DOTween.To(
            () => tilemap.color,      // 현재 값 가져오기
            x => tilemap.color = x,   // 값 적용
            endColor,                 // 목표 값
            fadeDuration              // 시간
        );
    }

    [PunRPC]
    void MoveToChat()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
