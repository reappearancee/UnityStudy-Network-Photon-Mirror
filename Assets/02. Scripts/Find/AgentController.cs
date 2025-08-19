using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    [SerializeField] private float wanderRadius = 30f;

    private float minWaitTime = 1f;
    private float maxWaitTime = 5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // anim = GetComponent<Animator>();
    }

    IEnumerator Start()
    {
        while (true)
        {
            SetRandomDestination();
            // anim.SetBool("IsWalk", true);
            
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
            
            // anim.SetBool("IsWalk", false);
            float idleTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(idleTime);
        }
    }
    
    private void SetRandomDestination()
    {
        var randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}