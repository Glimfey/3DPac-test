using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPathPacman : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask Obstacles;
    public LayerMask playersMask;

    public bool canSeePlayer;

    public Rigidbody target;
    public GameObject players;
    NavMeshAgent navMeshAgent;
    private float maxVelocity = 50.0f;
    private List<Vector3> findPosition;
    private int currentFindList;
    private bool findListPosition;

    Vector3 arrivalPoint;
    float slowingRadius = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        navMeshAgent = GetComponent<NavMeshAgent>();
        findPosition = new List<Vector3>();
        findPosition.Add(new Vector3(-1, 1, -1));
        findPosition.Add(new Vector3(21, 1, 10));
        findPosition.Add(new Vector3(22, 1, 61));
        findPosition.Add(new Vector3(-11, 1, 35));
        findPosition.Add(new Vector3(-41, 1, 12));
        findPosition.Add(new Vector3(-41, 1, -17));
        findPosition.Add(new Vector3(-41, 1, -31));
        findPosition.Add(new Vector3(-29, 1, -31));
        findPosition.Add(new Vector3(-31, 1, -18));
        findPosition.Add(new Vector3(-13, 1, -35));
        findListPosition = true;
        currentFindList = 0;
        arrivalPoint = new Vector3(4,1,26);
    }

    private IEnumerator FOVRoutine ()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        for(int i=0;i<2;i++)
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, viewRadius, playersMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, Obstacles))
                    {
                        canSeePlayer = true;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else if (canSeePlayer)
            {
                canSeePlayer = false;
            }
        }
            
    }

    // Update is called once per frame
    void Update()
    {   
        if (canSeePlayer)
        {
            Pursuit(target.position);
        }
        else
        {
            float distancePoint = Vector3.Distance(findPosition[currentFindList], transform.position);
            Seek(findPosition[currentFindList]);
            if (distancePoint <=2)
            {
                if (findListPosition)
                {
                    currentFindList++;
                    if (currentFindList == findPosition.Count-1)
                    {
                        findListPosition = false;
                    }
                }
                else
                {
                    currentFindList--;
                    if (currentFindList == 0)
                    {
                        findListPosition = true;
                    }
                }
            }
        }
        
    }

    private void Seek(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }
    private void Pursuit(Vector3 targetPos)
    {
        Vector3 distance = targetPos - transform.position;
        float timeUpdates = distance.magnitude / maxVelocity;
        Vector3 futurePosition = target.position + target.velocity * timeUpdates;
        Seek(futurePosition);
    }

    private void Arrival()
    {
        Vector3 desiredVelocity = (arrivalPoint - transform.position);
        float distance = desiredVelocity.magnitude;

        if (distance < slowingRadius)
        {
            desiredVelocity = desiredVelocity.normalized * maxVelocity * (distance / slowingRadius);
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * maxVelocity;
        }
        Seek(desiredVelocity);
    }
}
