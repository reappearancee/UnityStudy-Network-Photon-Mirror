using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviourPun
{
    private NavMeshAgent agent;
    private Animator anim;

    [SerializeField] private float wanderRadius = 30f;

    private float minWaitTime = 1f;
    private float maxWaitTime = 5f;

    [SerializeField] private float turnSpeed = 10f;

    private bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(WanderRoutine());
    }

    IEnumerator WanderRoutine()
    {
        while (!isDead)
        {
            var randomDir = Random.insideUnitSphere * wanderRadius;
            randomDir += transform.position;
            
            photonView.RPC(nameof(SetDestination), RpcTarget.AllBuffered, randomDir);
            
            float moveType = Random.Range(0, 2) == 0 ? 0.5f : 1f;
            anim.SetFloat("Speed", moveType); // 이동 애니메이션
            agent.speed = moveType * 4f; // 2 or 4

            
            yield return new WaitUntil(() => !isDead && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
            
            anim.SetFloat("Speed", 0f); // 정지 애니메이션

            float idleTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(idleTime);
        }
    }

    void Update()
    {
        Vector3 dir = agent.desiredVelocity;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
    }
    
    [PunRPC]
    private void SetDestination(Vector3 dir)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(dir, out hit, wanderRadius, NavMesh.AllAreas))
        {
            if (!isDead)
                agent.SetDestination(hit.position);
        }
    }

    public void GetHit()
    {
        photonView.RPC(nameof(Dead), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Dead()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        anim.SetTrigger("Death");
        agent.updatePosition = false;
        agent.isStopped = true;
    }
}