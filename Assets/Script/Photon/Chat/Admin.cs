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
    [SerializeField] bool bef = false;
    private void Update()
    {
        if (!bef && isAdmin)
        {
            bef = true;

            string msg = string.Format(
                "<color=#00FF00>관리자 권한이 </color><color=yellow>[{0}]</color><color=#00FF00>에게 부여되었습니다.</color>",
                me.NickName
            );

            cm.photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
            cm.ReceiveMsg(msg);
        }
        else if (bef && !isAdmin)
        {
            bef = false;

            string msg = string.Format(
                "<color=#FF5555>[{0}]의 관리자 권한이 해제되었습니다.</color>",
                me.NickName
            );

            cm.photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
            cm.ReceiveMsg(msg);
        }
    }

    public void Oper(string msg)
    {
        switch (msg[0])
        {
            case 'm':
                cm.photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg.Substring(1));
                break;
        }

    }
}
