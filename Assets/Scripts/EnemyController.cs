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
    public bool dead;

    // TODO: race condition!!!
    public EmptyDetect curr_collider;

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
        if (!dead)
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


	private void OnCollisionEnter(Collision collision)
	{
        if (!dead)
        {
            if (collision.transform.tag == "Bullet")
            {
                Die();
            }
        }
	}

	private void OnTriggerEnter(Collider other)
	{
        if (!dead)
        {
            if (other.transform.tag == "Bullet")
            {
                Die();
            }
        }
    }



	private void Die()
	{
        Destroy(transform.GetComponent<BoxCollider>());
        if (curr_collider != null) curr_collider.colliders_count--;

        dead = true;
        agent.isStopped = true;
		
        animator.SetBool("Run Forward", false);
        animator.SetTrigger("Take Damage");
        animator.SetTrigger("Die");
        StartCoroutine(waiter());
    }
    IEnumerator waiter()
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

}
