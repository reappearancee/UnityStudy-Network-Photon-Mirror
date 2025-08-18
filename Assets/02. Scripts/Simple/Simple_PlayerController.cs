using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Simple_PlayerController : MonoBehaviourPun
{
    private CharacterController cc;

    [SerializeField] private TextMeshPro nickName;
    [SerializeField] private GameObject hat;
    
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 10f;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        if (photonView.IsMine)
        {
            nickName.text = PhotonNetwork.NickName;
            nickName.color = Color.green;
        }
        else
        {
            nickName.text = photonView.Owner.NickName;
            nickName.color = Color.red;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Move();
            Turn();
        }
    }
    
    private void Move()
    {
        cc.Move(moveInput * moveSpeed * Time.deltaTime);
    }

    void OnMove(InputValue value)
    {
        var moveValue = value.Get<Vector2>();
        moveInput = new Vector3(moveValue.x, 0, moveValue.y);
    }

    void Turn()
    {
        if (moveInput == Vector3.zero)
            return;

        Quaternion targetRot = Quaternion.LookRotation(moveInput);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    void OnHatOn()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Hat", RpcTarget.All, true);
        }
    }
    
    void OnHatOff()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Hat", RpcTarget.All, false);
        }
    }

    [PunRPC]
    private void Hat(bool isOn)
    {
        hat.SetActive(isOn);
    }
}