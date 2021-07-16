using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{


    public NavMeshAgent agent;
    public Animator animator;

    private EnemySpawner spawnerRef;
    
    public Vector3 attakTarget;
    public Vector3 Destination;
    private bool reachedDestintaion;
    public bool dead;

    public bool target_less;

    // TODO: race condition!!!
    public EmptyDetect curr_collider;


    public HealthBar health;
    // Start is called before the first frame update
    void Start()
    {
        reachedDestintaion = false;
        health = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        target_less = false;
    }

    
    public void SetTarget(Vector3 building, Vector3 slot, EnemySpawner creator)
	{
        attakTarget = building;
        Destination = slot;
        spawnerRef = creator;

        if (agent != null)
        {
            //TODO: Race condition when erasing while setting target?
            agent.SetDestination(Destination);
        }
        target_less = false;
    }




    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
        //    if (target_less)
        //    {
        //        StartCoroutine(circle());
        //    }
            if (!agent.pathPending)
            {
                // Running animation
                animator.SetBool("Run Forward", true);
                agent.isStopped = false;
                reachedDestintaion = false;
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
                        StartCoroutine(waitAttack());


                        agent.isStopped = true;
                        reachedDestintaion = true;

                    }
                }
                
            }
        }

    }

    //IEnumerator circle()
    //{
    //    animator.SetBool("Run Forward", true);
    //    agent.isStopped = false;
    //    reachedDestintaion = false;
    //    attakTarget += Vector3.one * 300;
    //    Destination += Vector3.one * 300;
    //    agent.SetDestination(Destination);
    //    yield return new WaitForSeconds(3);
    //    attakTarget += Vector3.one * -300;
    //    Destination += Vector3.one * -300;
    //    agent.SetDestination(Destination);
    //}

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
        spawnerRef.RemoveEnemy(this);
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

    IEnumerator waitAttack()
	{
        yield return new WaitForSeconds(2);
        health.SetHealthRelative(0.01f);
	}

}
