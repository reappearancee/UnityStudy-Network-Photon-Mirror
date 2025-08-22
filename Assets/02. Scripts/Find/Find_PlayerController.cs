using System.Collections;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Find_PlayerController : MonoBehaviourPun
{
    private Animator anim;

    [SerializeField] private Transform playerRoot;
    
    [SerializeField] private GameObject punchBox;
    [SerializeField] private GameObject kickBox;
    
    private bool isAttack = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            var followCamera = FindFirstObjectByType<CinemachineCamera>();
            followCamera.Target.TrackingTarget = playerRoot;
        }
        else
        {
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    void OnPunch()
    {
        if (!isAttack)
            photonView.RPC(nameof(RPC_Punch), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Punch()
    {
        StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        isAttack = true;
        anim.SetTrigger("Punch");
        yield return new WaitForSeconds(0.5f);
        punchBox.SetActive(true);
        
        yield return new WaitForSeconds(0.3f);
        punchBox.SetActive(false);
        isAttack = false;
    }

    void OnKick()
    {
        if (!isAttack)
            photonView.RPC(nameof(RPC_Kick), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Kick()
    {
        StartCoroutine(KickRoutine());
    }
    
    IEnumerator KickRoutine()
    {
        isAttack = true;
        anim.SetTrigger("Kick");
        yield return new WaitForSeconds(0.6f);
        kickBox.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        kickBox.SetActive(false);
        isAttack = false;
    }
}