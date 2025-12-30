using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;

public class TLqkf : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI text;
    int cnt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Update()
    {
        text.text = cnt.ToString();
    }

    // Update is called once per frame
    public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
    {
        cnt = roomInfos.Count;
    }
}
