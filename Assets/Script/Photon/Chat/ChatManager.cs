using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using System;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public Button sendBtn;
    public TextMeshProUGUI chatLog;
    public TextMeshProUGUI channel;
    public TMP_InputField inputField;
    public TextMeshProUGUI playerList;
    public Admin admin;
    string players;
    public ScrollRect scroll_rect = null;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
        channel.text = PhotonNetwork.CurrentRoom.Name;
    }

    void Update()
    {
        ChatterUpdate();
        if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown(KeyCode.KeypadEnter) && inputField.isFocused))
        {
            inputField.text += " ";
            SendButtonOnClicked();
        }
    }

    IEnumerator Send(string msg)
    {
        yield return new WaitForSeconds(.05f);

        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        inputField.ActivateInputField();
    }
    public void SendButtonOnClicked()
    {
        if (inputField.text.Equals(""))
        {
            Debug.Log("Empty");
            return;
        }
       
        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, inputField.text + Input.compositionString);
        inputField.text = "";
        if (admin.isAdmin)
            admin.Oper(msg);
        else
            StartCoroutine(Send(msg));

    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0); 
    }

    void ChatterUpdate()
    {
        players = "참가자 목록\n";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            players += p.NickName + "\n";
        }
        playerList.text = players;
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ReceiveMsg($"이제 이전 방장은 죽고 <color=yellow>[{newMasterClient.NickName}]의 시대가 되었다.</color>");
    }

    public void GameStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("OnGameRoom", RpcTarget.AllBuffered);
        }
        else
        {
            Debug.Log("마스터 클라이언트가 아님");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        string msg = string.Format("<color=#00ff00>[{0}]님이 {1}에 갓 탄생하셨습니다.</color>", newPlayer.NickName, DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
        ReceiveMsg(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        string msg = string.Format("<color=#ff0000>[{0}]님이 {1}에 사망하셨습니다.</color>", otherPlayer.NickName, DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
        ReceiveMsg(msg);
    }

    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        StartCoroutine(ScrollUpdate());
    }

    [PunRPC]
    public void OnGameRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }

    IEnumerator ScrollUpdate()
    {
        yield return null;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
