using System;
using Photon.Pun;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] private PhotonView myPv;
    [SerializeField] private float damage;

    void Awake()
    {
        myPv = transform.root.GetComponent<PhotonView>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView otherPv = other.GetComponent<PhotonView>();

            if (myPv.IsMine)
                myPv.RPC("TriggerEvent", RpcTarget.All, otherPv.ViewID, damage);
        }
    }
}