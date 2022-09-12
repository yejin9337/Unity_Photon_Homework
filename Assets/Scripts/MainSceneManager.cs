using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainSceneManager : MonoBehaviourPun
{
    private void Awake()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
}
