using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Find_GameManager : Singleton<Find_GameManager>
{
    IEnumerator Start()
    {
        float randomTime = Random.Range(0f, 1f);
        yield return new WaitForSeconds(randomTime);
        
        int ranIndex = Random.Range(0, 5); // 0, 1, 2, 3, 4
        
        var randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        
        PhotonNetwork.Instantiate("Player_" + ranIndex, randomPos, Quaternion.identity);
    }
}