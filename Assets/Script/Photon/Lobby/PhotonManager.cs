using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Text.RegularExpressions;
using Photon.Pun.UtilityScripts;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI connectionStatus;
    public TextMeshProUGUI idText;
    public Button loginBtn;
    public Button createBtn;
    public TMP_InputField inputField;
    public GameObject input;
    public InputManager inputManager;
    public GameObject roomListPanel;
    public Transform roomListContent;
    public GameObject roomItemPrefab;

    [SerializeField]
    private Dictionary<string, RoomInfo> roomList = new Dictionary<string, RoomInfo>();

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        loginBtn.interactable = false;
        createBtn.interactable = false;
        connectionStatus.text = "연결 중..";
    }
    public void Create()
    {
        input.gameObject.SetActive(true);
        createBtn.interactable = false;

        inputManager.RequestInput(value =>
        {
            PhotonNetwork.LocalPlayer.NickName = idText.text.Replace("_", "").Replace("\0", "").Trim();
            if (PhotonNetwork.IsConnected)
            {
                connectionStatus.text = "방 생성 중..";
                PhotonNetwork.CreateRoom(value, new RoomOptions { MaxPlayers=6});
                PhotonNetwork.AutomaticallySyncScene = true;
                input.gameObject.SetActive(false);

            }
            else
            {
                connectionStatus.text = "(오프라인) 연결 실패\n재시도 중..";
                PhotonNetwork.ConnectUsingSettings();
            }
        },
        () =>
        {
            createBtn.interactable = true;
            input.gameObject.SetActive(false);
        });
    }
    public void Connect()
    {
        string cleanedInput = idText.text.Replace("_", "").Replace("\0", "").Trim();

        if (cleanedInput.Equals(""))
        {
            Debug.Log("닉네임 입력 안 됨");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = cleanedInput;
        loginBtn.interactable = false;
        createBtn.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionStatus.text = "방 목록 불러오는 중..";
            OpenRoomList();   // ⭐ 여기서 패널 열기
        }
        else
        {
            connectionStatus.text = "(오프라인) 연결 실패\n재시도 중..";
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    public void OpenRoomList()
    {
        roomListPanel.SetActive(true);
        RefreshRoomListUI();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
    {
        foreach (RoomInfo info in roomInfos)
        {
            if (info.RemovedFromList)
                roomList.Remove(info.Name);
            else
                roomList[info.Name] = info;
        }

        RefreshRoomListUI();
        UpdateLoginButton();
    }

    void RefreshRoomListUI()
    {
        foreach (Transform child in roomListContent)
            Destroy(child.gameObject);

        foreach (RoomInfo room in roomList.Values)
        {
            GameObject item = Instantiate(roomItemPrefab, roomListContent);
            RoomItemUI ui = item.GetComponent<RoomItemUI>();

            ui.SetRoom(room);
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        createBtn.interactable = true;
        connectionStatus.text = "로비 접속 중..";
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        loginBtn.interactable = false;
        createBtn.interactable = false;
        connectionStatus.text = "(오프라인) 연결 실패\n재시도 중..";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionStatus.text = "새 방 생성 중..";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8 });
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedRoom()
    {
        roomListPanel.SetActive(false);
        connectionStatus.text = "방 참가 성공";
        PhotonNetwork.LoadLevel(1);
    }
    void UpdateLoginButton()
    {
        bool hasRoom = roomList.Count > 0;

        loginBtn.interactable = hasRoom;
        createBtn.interactable = PhotonNetwork.IsConnected;

        if (!hasRoom)
            connectionStatus.text = "대기 중";
    }
}