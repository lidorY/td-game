using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{


    public NavMeshAgent agent;
    public Animator animator;

    private EnemySpawner spawnerRef;
    
    private Vector3 attakTarget;
    private Vector3 Destination;
    private bool reachedDestintaion;



    // Start is called before the first frame update
    void Start()
    {
        reachedDestintaion = false;
        attakTarget = GameObject.Find("Castle").transform.position;
        spawnerRef = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        Destination = spawnerRef.AttackSlot();
        agent.SetDestination(Destination);
    }

    

    // Update is called once per frame
    void Update()
    {
		if (!reachedDestintaion)
		{
            // Running animation
            animator.SetBool("Run Forward", true);
		}

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Face target direction
                    Vector3 dir = attakTarget - transform.position;
                    dir.y = 0;//This allows the object to only rotate on its y axis
                    Quaternion rot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rot, 3 * Time.deltaTime);
                    animator.SetBool("Run Forward", false);
                    animator.SetTrigger("Attack 01");
                    agent.isStopped = true;
                    reachedDestintaion = true;

                }
            }
        }

    }

}
