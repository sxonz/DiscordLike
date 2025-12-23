using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class Admin : MonoBehaviourPunCallbacks
{
    public bool isAdmin = false;
    Player me;
    public ChatManager cm;
    public bool IsAdmin()
    {
        #if UNITY_EDITOR
            return isAdmin;
        #else
            return false;
        #endif
    }
    private void Start()
    {
        me = PhotonNetwork.LocalPlayer;
    }
    [SerializeField]bool bef = false;
    private void Update()
    {
        if (!bef && isAdmin)
        {
            bef = isAdmin;
            string msg = string.Format("<color=#0000FF>[{0}]</color>님이 <color=#00FFFF>관리자가 되었습니다!!</color>", me.NickName);

            cm.photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
            cm.ReceiveMsg(msg);
        }
        else if(bef && !isAdmin)
        {
            bef = isAdmin;
            string msg = string.Format("[{0}] 님이<color=#FF0000> 죄수가 되었습니다.</color>", me.NickName);
            cm.photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
            cm.ReceiveMsg(msg);
        }
    }
    public void Oper(string msg)
    {
        switch (msg[0])
        {
            case 'm':
                cm.photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered,msg.Substring(1));
                break;
        }
        
    }
    void Kick(string name)
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == name)
            {
                
            }
        }
    }
}
