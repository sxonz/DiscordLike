using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LeaveRoomUI : MonoBehaviour
{
    public GameObject confirmPanel;
    public Button yesButton;
    public Button noButton;

    void Start()
    {
        confirmPanel.SetActive(false);

        yesButton.onClick.AddListener(ConfirmLeave);
        noButton.onClick.AddListener(CancelLeave);
    }

    public void OpenConfirm()
    {
        confirmPanel.SetActive(true);
    }

    void CancelLeave()
    {
        confirmPanel.SetActive(false);
    }

    void ConfirmLeave()
    {
        PhotonNetwork.LeaveRoom();
        confirmPanel.SetActive(false);
    }
}
