using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class StartButtonUI : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public GameObject gameChoice;
    public GameChoice choice;

    void Start()
    {
        UpdateButton();
        startButton.onClick.AddListener(StartGame);
    }

    void UpdateButton()
    {
        startButton.interactable =
            PhotonNetwork.IsMasterClient &&
            PhotonNetwork.PlayerList.Length >= 1 &&
            AllPlayersReady();
    }

    void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (PhotonNetwork.PlayerList.Length < 1)
            return;

        if (!AllPlayersReady())
            return;

        gameChoice.SetActive(true);
        choice.Choice(GameCB);
    }
    
    void GameCB(int roomID)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel(roomID);
    }

    bool AllPlayersReady()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!p.CustomProperties.TryGetValue("Ready", out object r) || !(bool)r)
                return false;
        }
        return true;
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready"))
            UpdateButton();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateButton();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateButton();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdateButton();
    }
}
