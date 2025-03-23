using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerMan;
    public GameObject PlayerMonster;
    public GameObject _cameraLoading;
    public TextMeshProUGUI _countPlayers;

    public ScoreScript[] Players;

    [Space]
    public int isClass;
    public GameObject[] Maps;
    /// <summary>
    /// 0 - oldMap
    /// 1 - desertMap
    /// 2 - summerMap
    /// 3 - autumnMap
    /// 4 - winterMap
    /// 5 - hellMap
    /// 6 - swampMap
    /// </summary>

    public Transform[] PintsOfSpawn;

    private void Start()
    {
        for (int i = 0; i < Maps.Length; i++) Maps[i].SetActive(false);
        isClass = PlayerPrefs.GetInt("ClassSelect");
        int _mapLoad = MapLoad(PhotonNetwork.CurrentRoom.Name);
        Maps[_mapLoad].SetActive(true);

        _mapLoad = _mapLoad * 2;
        if (isClass == 0) PhotonNetwork.Instantiate(playerMan.name, new Vector3(PintsOfSpawn[_mapLoad].position.x + Random.Range(-5, 5), 0, 0), Quaternion.identity);
        else PhotonNetwork.Instantiate(PlayerMonster.name, new Vector3(PintsOfSpawn[_mapLoad+1].position.x + Random.Range(-5, 5), 5, 0), Quaternion.identity);
    }

    private void Update()
    {
        _countPlayers.text = PhotonNetwork.CountOfPlayersInRooms.ToString() + " players now";
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SpawnAndSetSide(string nameSide)
    {
        if(nameSide=="man") PhotonNetwork.Instantiate(playerMan.name, new Vector3(PintsOfSpawn[0].position.x + Random.Range(-2, 2), 0, 0), Quaternion.identity);
        else PhotonNetwork.Instantiate(PlayerMonster.name, new Vector3(PintsOfSpawn[1].position.x + Random.Range(-2, 2), 5, 0), Quaternion.identity);
        
        Destroy(_cameraLoading);
    }
        
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
    }

    public int MapLoad(string value)
    {
        int Output = 0;
        if (value == "0") Output = 0;
        else if (value == "1") Output = 1;
        else if (value == "2") Output = 2;
        else if (value == "3") Output = 3;
        else if (value == "4") Output = 4;
        else if (value == "5") Output = 5;
        else if (value == "6") Output = 6;
        return Output;
    }
}
