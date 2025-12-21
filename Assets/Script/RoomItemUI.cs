using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomItemUI : MonoBehaviour
{
    public TMP_Text roomNameText;
    public TMP_Text playerCountText;
    public Button joinButton;

    RoomInfo roomInfo;

    public void SetRoom(RoomInfo info)
    {
        roomInfo = info;

        roomNameText.text = info.Name;
        playerCountText.text = $"{info.PlayerCount} / {info.MaxPlayers}";

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(JoinRoom);
    }

    void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
}
