using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Start");
        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = "1.0";
        // 설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버 접속 성공 시 자동 실행
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("OnConnectedToMaster");
    }


    int tryConnectCount = 0;
    // 마스터 서버 접속 실패 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        ++tryConnectCount;
        if (tryConnectCount > 5)
        {
            Debug.Log("서버에 문제가 있어 접속할 수 없습니다.");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        Debug.Log("OnDisconnected");
    }

    // 로비에 접속하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        Connect();
    }

    // 룸 접속 시도
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinRoom");
        PhotonNetwork.LoadLevel("Main");
    }
}
