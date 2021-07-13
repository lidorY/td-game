using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyDetect : MonoBehaviour
{

    public int colliders_count = 0;

    public float enemy_detection_radius = 3;

    public bool collided;
    // Start is called before the first frame update
    void Start()
    {
        colliders_count = 0;
        collided = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliders_count > 0)
        {
            collided = true;
        }
		else
		{
            colliders_count = 0;
            collided = false;
		}
    }


	private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Ground" && other.transform.tag != "Bullet" && other.transform.tag != "Enemy")
        {
           

                colliders_count++;
                //collided = true;
            
        }
    }

	private void OnTriggerStay(Collider other)
	{
        if (other.transform.tag == "Enemy")
		{
            
            if (Vector3.Distance(transform.position, other.transform.position) < enemy_detection_radius)
            {
                if (other.transform.GetComponent<EnemyController>().curr_collider == null)
                {
                    other.transform.GetComponent<EnemyController>().curr_collider = this;
                    colliders_count++;
                }
            }
            else
            {
                if(other.transform.GetComponent<EnemyController>().curr_collider != null)
				{
                    other.transform.GetComponent<EnemyController>().curr_collider = null;
                    colliders_count--;
				}
            }
        }

    }

	private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Ground" && other.transform.tag != "Bullet" & other.transform.tag != "Enemy")
        {
           colliders_count--;           
        }
    }
}
