using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	public Transform enemyRef;
	public float spawnDistance;

	public float timeout;

	private float timeCounter;

	// Start is called before the first frame update
	void Start()
	{
		timeCounter = 0;
	}

	// Update is called once per frame
	void Update()
	{
		timeCounter += Time.deltaTime;
		if (timeCounter > timeout)
		{
			float theta = Random.value * 360;
			float x = spawnDistance * Mathf.Sin(theta);
			float y = spawnDistance * Mathf.Cos(theta);
			Vector3 spawn_position = new Vector3(x, 0.5f, y);


			Instantiate(enemyRef, spawn_position, Quaternion.identity);
			timeCounter = 0;
		}
	}

}

