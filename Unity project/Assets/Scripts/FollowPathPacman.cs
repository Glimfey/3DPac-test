using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPathPacman : MonoBehaviour
{
    public Transform target;
    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(true)
        {
            navMeshAgent.SetDestination(target.position);
        }
        else
        {

        }
    }
}
