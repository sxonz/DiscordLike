using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using System;
using ExitGames.Client.Photon;

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
        players = "������ ���\n";

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            bool isReady = false;

            if (p.CustomProperties.ContainsKey("IsReady"))
                isReady = (bool)p.CustomProperties["IsReady"];

            if (isReady)
                players += $"<color=#00ff00>{p.NickName}</color>\n";
            else
                players += p.NickName + "\n";
        }

        playerList.text = players;
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ReceiveMsg($"���� ���� ������ �װ� <color=yellow>[{newMasterClient.NickName}]�� �ô밡 �Ǿ���.</color>");
    }


    public void GameStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("OnGameRoom", RpcTarget.AllBuffered);
        }
        else
        {
            Debug.Log("������ Ŭ���̾�Ʈ�� �ƴ�");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        string msg = string.Format("<color=#00ff00>[{0}]���� {1}�� �� ź���ϼ̽��ϴ�.</color>", newPlayer.NickName, DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
        ReceiveMsg(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        string msg = string.Format("<color=#ff0000>[{0}]���� {1}�� ����ϼ̽��ϴ�.</color>", otherPlayer.NickName, DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
        ReceiveMsg(msg);
    }
    public void ToggleReady()
    {
        bool isReady = false;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IsReady"))
            isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsReady"];

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["IsReady"] = !isReady;

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
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
