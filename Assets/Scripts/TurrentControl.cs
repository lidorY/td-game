using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentControl : MonoBehaviour
{
    List<Transform> targets;

    [SerializeField] private Transform Turrent;
    [SerializeField] private Transform Bullet;
    [SerializeField] private Transform GunEndpoint;

    public float timeout;
    private float timeCounter;


    private Transform targetRef = null;
    // Start is called before the first frame update
    void Start()
    {
        targets = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        timeCounter += Time.deltaTime;

        if (!targetRef)
        {
            // Acquire new target
            foreach (var t in targets)
            {
				if (!t) { continue; }
				if (t.GetComponent<EnemyController>().dead) { continue; }
                Vector3 dir = t.position - Turrent.position;
                Quaternion rot = Quaternion.LookRotation(dir);
                if (rot.eulerAngles.x <= 30)
                {
                    targetRef = t;
                    break;
                }
            }
        }
		else
		{
            if (!targetRef) { return; }
            if (targetRef.GetComponent<EnemyController>().dead) {
                targetRef = null;
                return; 
            }
            Vector3 dir = targetRef.position - Turrent.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            if (rot.eulerAngles.x > 30)
            {
                targetRef = null;
            }
			else
			{
                // Face target direction
                Turrent.rotation = Quaternion.Lerp(Turrent.localRotation, rot, 5 * Time.deltaTime);

                if (timeCounter > timeout)
                {
                    Transform bullet = Instantiate(Bullet, GunEndpoint.position, Quaternion.identity);
                    bullet.GetComponent<Bullet>().Setup(dir);

                    timeCounter = 0;
                }
            }
        }
    }


	private void OnTriggerEnter(Collider other)
	{
        if (other.transform.tag == "Enemy")
        {
            targets.Add(other.transform);
        }
    }

	private void OnTriggerExit(Collider other)
	{
        if(targetRef == other.transform)
		{
            targetRef = null;
		}
        targets.Remove(other.transform);
    }


	public void SelfDestroy()
	{
        Destroy(gameObject);
	}


}
