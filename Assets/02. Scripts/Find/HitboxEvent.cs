using System;
using UnityEngine;

public class HitboxEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            other.GetComponent<AgentController>().GetHit();
        }
        else if (other.CompareTag("Player"))
        {
            
        }
    }
}