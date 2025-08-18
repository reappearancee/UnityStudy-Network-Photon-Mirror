using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Simple_NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false); // 해상도 설정
        PhotonNetwork.SendRate = 60; // 내 컴퓨터 게임 정보에 대한 전송률
        PhotonNetwork.SerializationRate = 30; // Photon View 관측 중인 대상에 대한 전송률
        PhotonNetwork.GameVersion = gameVersion; // 버전 설정
    }

    void Start()
    {
        Connect();
    }

    private void Connect()
    {
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
        PhotonNetwork.Instantiate("Player", Vector3.up, Quaternion.identity);
        Debug.Log("캐릭터 생성");
    }
}