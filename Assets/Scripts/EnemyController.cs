using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    public float speed;
    private Vector3 Destination;
    private Vector3 Origin;
    private float DistanceFromDest;

    // Start is called before the first frame update
    void Start()
    {
        Destination = new Vector3(0, 0.5f, 0);
        DistanceFromDest = Vector3.Distance(transform.localPosition, Destination);
        Origin = transform.localPosition;

        StartCoroutine(moveObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator moveObject()
	{
        float toatlTime = DistanceFromDest / speed;
        float currentTime = 0f;
        while(Vector3.Distance(transform.localPosition, Destination) > 0)
		{
            currentTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(Origin, Destination, currentTime/ toatlTime);
            yield return null;
		}
        Destroy(gameObject);
	}
}
