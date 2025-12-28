using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Unity.Mathematics;

public class ReadyButtonUI : MonoBehaviourPunCallbacks
{
    public Button readyButton;
    public TextMeshProUGUI readyText;
    public TextMeshProUGUI readyCountText;

    private bool isReady = false;

    void Start()
    {
        readyButton.onClick.AddListener(ToggleReady);
        UpdateReadyCount();
    }

    void ToggleReady()
    {
        isReady = !isReady;

        Hashtable props = new Hashtable
        {
            { "Ready", isReady }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        readyText.text = isReady ? "준비 완료" : "플레이 준비";
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready"))
            UpdateReadyCount();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateReadyCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateReadyCount();
    }

    void UpdateReadyCount()
    {
        int ready = 0;
        int total = PhotonNetwork.CurrentRoom.PlayerCount;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("Ready", out object r) && (bool)r)
                ready++;
        }

        readyCountText.text = $"{ready} / {total}";
    }
}
