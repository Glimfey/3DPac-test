using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPathGhost : MonoBehaviour
{
    int stamina = 0;
    int staminaMax = 1000;
    public Transform target;
    NavMeshAgent navMeshAgent;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.Angle(this.transform.position, rb.position);
        float distance = Vector3.Distance(target.transform.position, navMeshAgent.transform.position);
        target.position = target.position;
        if (distance <= 10 && stamina <= staminaMax)
        {
            navMeshAgent.SetDestination(target.position);
            stamina++;
        }
        else
        {

        }
    }
}
