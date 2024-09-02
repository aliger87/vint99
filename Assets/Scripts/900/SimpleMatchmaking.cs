using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SimpleMatchmaking : MonoBehaviourPunCallbacks
{
    public GameObject matchingPanel;
    public TMP_Text matchingText;
    private int matchTimer;

    private void Start()
    {
        matchingPanel.SetActive(false);
        matchTimer = 0;
    }

    public void OnMatchmakingButtonClicked()
    {
        matchingPanel.SetActive(true);
        matchTimer = 0;
        matchingText.text = "Searching for a match...";
        InvokeRepeating("UpdateMatchTimer", 1f, 1f);
        PhotonNetwork.JoinRandomRoom();
    }

    private void UpdateMatchTimer()
    {
        matchTimer++;
        matchingText.text = "Searching for a match... " + matchTimer + "s";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        matchingText.text = "No matches found, creating a new match...";
        CancelInvoke("UpdateMatchTimer");
        CreateNewGame();
    }

    public override void OnJoinedRoom()
    {
        matchingText.text = "Match found, starting the game...";
        CancelInvoke("UpdateMatchTimer");
        StartGame();
    }

    private void CreateNewGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    private void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.LoadLevel("1");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        matchingText.text = "Player found! Starting the game...";
        StartGame();
    }
}
