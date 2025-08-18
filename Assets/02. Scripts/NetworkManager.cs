using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    [SerializeField] private TMP_InputField nickNameField;
    [SerializeField] private Button connectButton;

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.GameVersion = gameVersion;
    }

    void Start()
    {
        connectButton.onClick.AddListener(Connect);
    }

    private void Connect()
    {
        PhotonNetwork.NickName = nickNameField.text;
        
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("서버 접속");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 20 }, null);
        Debug.Log("서버 접속 완료");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
        Debug.Log("씬 전환");
    }
}