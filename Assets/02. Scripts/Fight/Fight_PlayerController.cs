using System.Collections;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Fight_PlayerController : MonoBehaviourPun
{
    private Animator anim;
    private PlayerInput playerInput;
    [SerializeField] private TextMeshPro nickName;
    [SerializeField] private Transform playerRoot;

    [SerializeField] private GameObject punchBox;
    [SerializeField] private GameObject kickBox;

    [SerializeField] private Image hpBar;
    private float currentHp = 100f;
    private float maxHp = 100f;

    private bool isAttack = false;
    public bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        currentHp = maxHp;

        if (photonView.IsMine)
        {
            var followCamera = FindFirstObjectByType<CinemachineCamera>();
            followCamera.Target.TrackingTarget = playerRoot;

            nickName.text = PhotonNetwork.NickName;
            nickName.color = Color.green;
        }
        else
        {
            nickName.text = photonView.Owner.NickName;
            nickName.color = Color.red;
            
            playerInput.enabled = false;
        }
    }

    void OnPunch()
    {
        if (!isAttack && !isDead)
            photonView.RPC("Attack", RpcTarget.All, "Punch", 0.5f, 0.3f, 0);
    }

    void OnKick()
    {
        if (!isAttack && !isDead)
            photonView.RPC("Attack", RpcTarget.All, "Kick", 0.6f, 0.2f, 1);
    }

    [PunRPC]
    private void Attack(string parameter, float playTime, float endTime, int hitBoxIndex)
    {
        StartCoroutine(AttackRoutine(parameter, playTime, endTime, hitBoxIndex));
    }

    IEnumerator AttackRoutine(string parameter, float playTime, float endTime, int hitBoxIndex)
    {
        GameObject hitBox = hitBoxIndex == 0 ? punchBox : kickBox;

        isAttack = true;
        anim.SetTrigger(parameter);

        yield return new WaitForSeconds(playTime);
        hitBox.SetActive(true);

        yield return new WaitForSeconds(endTime);
        hitBox.SetActive(false);
        isAttack = false;
    }

    [PunRPC]
    private void TriggerEvent(int viewID, float damage)
    {
        PhotonView targetPv = PhotonView.Find(viewID);

        if (targetPv != null)
            targetPv.GetComponent<Fight_PlayerController>().GetDamage(damage);
    }

    public void GetDamage(float damage)
    {
        currentHp -= damage;

        hpBar.fillAmount = currentHp / maxHp;

        if (currentHp <= 0f)
        {
            if (photonView.IsMine)
            {
                isDead = true;
                anim.SetTrigger("Death");
                GetComponent<CharacterController>().enabled = false;
                Fight_GameManager.Instance.EndGame();
            }
        }
    }
}