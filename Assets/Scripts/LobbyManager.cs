using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField nick;
    public GameObject PanelLoading;
    public GameObject PanelCreateRoom;
    public TextMeshProUGUI chatText;
    public int MapValue = 0;
    public bool RandomMap = true;

    private void Start()
    {
        PanelLoading.SetActive(true);
        PhotonNetwork.NickName = "Player" + Random.Range(0, 9999);
        if (PlayerPrefs.HasKey("Nick")) nick.text = PlayerPrefs.GetString("Nick");

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.8.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        chatText.text = PhotonNetwork.CountOfPlayersInRooms.ToString() + " players now";
    }

    public override void OnConnectedToMaster()
    {
        PanelLoading.SetActive(false);
    }

    public void PlayToGame()
    {
        PanelLoading.SetActive(true);
        if (nick.text.Length > 0) PhotonNetwork.NickName = nick.text;
        PlayerPrefs.SetString("Nick", nick.text);
        PhotonNetwork.JoinRandomRoom();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PanelLoading.SetActive(false);
        PanelCreateRoom.SetActive(true);
    }

    public void CreateRoomButton()
    {
        PanelLoading.SetActive(true);
        PhotonNetwork.CreateRoom(MapValue.ToString(), new Photon.Realtime.RoomOptions { MaxPlayers = 20, CleanupCacheOnLeave = true });
    }
}
