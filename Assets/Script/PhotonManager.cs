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
    public TMP_InputField inputField;
    public GameObject input;
    public InputManager inputManager;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        loginBtn.interactable = false;
        connectionStatus.text = "연결 중..";
    }
    public void Create()
    {
        input.gameObject.SetActive(true);
       
        inputManager.RequestInput(value =>
        {
            PhotonNetwork.LocalPlayer.NickName = idText.text.Replace("_", "").Replace("\0", "").Trim();
            PhotonNetwork.CreateRoom(value);
            input.gameObject.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                connectionStatus.text = "방 생성 중..";
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                connectionStatus.text = "(오프라인) 연결 실패\n재시도 중..";
                PhotonNetwork.ConnectUsingSettings();
            }
        },
        () =>
        {
            input.gameObject.SetActive(false);
        });
    }
    public void Connect()
    {
        string cleanedInput = idText.text.Replace("_", "").Replace("\0", "").Trim();

        if (cleanedInput.Equals(""))
        {
            Debug.Log("넌 나가라");
            return;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = cleanedInput;
            loginBtn.interactable = false;

            if (PhotonNetwork.IsConnected)
            {
                connectionStatus.text = "방 입장 중..";
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                connectionStatus.text = "(오프라인) 연결 실패\n재시도 중..";
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        loginBtn.interactable = true;
        connectionStatus.text = "연결됨";
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        loginBtn.interactable = false;
        connectionStatus.text = "(오프라인) 연결 실패\n재시도 중..";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionStatus.text = "새 방 생성 중..";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8 });
    }

    public override void OnJoinedRoom()
    {
        connectionStatus.text = "참가 성공";
        PhotonNetwork.LoadLevel(1);
    }
}