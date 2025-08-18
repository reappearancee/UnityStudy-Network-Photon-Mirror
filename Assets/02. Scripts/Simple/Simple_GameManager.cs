using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Simple_GameManager : MonoBehaviourPun
{
    IEnumerator Start()
    {
        yield return null;
        
        PhotonNetwork.Instantiate("Player", Vector3.up, Quaternion.identity);
    }
}