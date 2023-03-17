using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FollowPathGhost : MonoBehaviour
{
    private int stamina = 0;
    private int staminaMax = 2500;
    private int staminaReset = 0;
    private float maxVelocity = 20.0f;
    private float distanceOthers = 10.0f;
    NavMeshAgent navMeshAgent;
    public Rigidbody target;
    public Rigidbody pacMan;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        //float angle = Vector3.Angle(this.transform.position, target.position);
        float distanceTarget = Vector3.Distance(target.transform.position, navMeshAgent.transform.position);
        float distancePacman = Vector3.Distance(pacMan.transform.position, navMeshAgent.transform.position);
        //run from pacman      
        if (distancePacman < distanceOthers)
        {
            Evade(pacMan.position);
        }
        //hunt player
        else if (distanceTarget <= distanceOthers && stamina <= staminaMax)
        {

            Pursuit(target.position);
            stamina++;
        }
        //standard wander
        else
        {
            Wander();
            staminaReset++;
        }
        if (staminaReset >= staminaMax)
        {
            staminaReset = 0;
            stamina = 0;
        }
    }

    //seek for pursuit & wander
    private void Seek(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
        //works but not really
        /*
        Vector3 desiredVelocity = (targetPosition - transform.position);
        desiredVelocity = desiredVelocity.normalized * maxVelocity * Time.deltaTime;

        //determine new direction from straight to target minus current direction
        Vector3 steering = desiredVelocity - velocity;
        velocity = Vector3.ClampMagnitude(velocity + steering, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        return transform.position;
        */
    }
    //flee for evade
    private void Flee(Vector3 targetPosition)
    {
        Vector3 FleeVector = targetPosition - this.transform.position;
        navMeshAgent.SetDestination(this.transform.position - FleeVector);
        //works but not really
        /*
        Vector3 direction = transform.position - pacMan.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        Vector3 desiredVelocity = (transform.position - targetPosition);
        desiredVelocity = desiredVelocity.normalized * maxVelocity * Time.deltaTime;

        Vector3 steering = desiredVelocity - velocity;
        velocity = Vector3.ClampMagnitude(velocity + steering, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        return transform.position;
        */
    }

    //pursuit player
    private void Pursuit(Vector3 targetPos)
    {
        Vector3 distance = targetPos - transform.position;
        float timeUpdates = distance.magnitude / maxVelocity;
        Vector3 futurePosition = target.position + target.velocity * timeUpdates;
        Seek(futurePosition);
    }

    //evade pacman
    private void Evade(Vector3 runFrom)
    {
        Vector3 distance = runFrom - transform.position;
        float timeUpdates = distance.magnitude / maxVelocity;
        Vector3 futurePosition = runFrom + pacMan.velocity * timeUpdates;
        Flee(futurePosition);
    }

    //wander
    Vector3 wanderTarget = Vector3.zero;
    private void Wander()
    {

        float circleRadius = 10;
        float circleDistance = 5;
        float wanderJitter = 2;
        //determine random target to move to
        wanderTarget = new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= circleRadius;
        //place the new location in the right place
        Vector3 targetPos = wanderTarget + new Vector3(0, 0, circleDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetPos);
        Seek(targetWorld);


    }
}
