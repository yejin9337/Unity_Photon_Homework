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
        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = "1.0";
        // ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("OnConnectedToMaster");
    }


    int tryConnectCount = 0;
    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        ++tryConnectCount;
        if (tryConnectCount > 5)
        {
            Debug.Log("������ ������ �־� ������ �� �����ϴ�.");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        Debug.Log("OnDisconnected");
    }

    // �κ� �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        Connect();
    }

    // �� ���� �õ�
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

    // �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinRoom");
        PhotonNetwork.LoadLevel("Main");
    }
}
