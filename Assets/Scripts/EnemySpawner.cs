using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	public Transform enemyRef;
	public Transform attakTarget;
	public uint positionsNum;
	public float radiusFromTarget;

	private List<Vector3> positionsAroundTarget;
	private int positionIndex = 0;

	public float spawnDistance;
	public float timeout;

	private float timeCounter;

	public Vector3 AttackSlot()
	{
		return positionsAroundTarget[positionIndex];
	}


	// Start is called before the first frame update
	void Start()
	{
		positionsAroundTarget = new List<Vector3>();
		float angleDiff = 360.0f / positionsNum;
		for (int i = 0; i < positionsNum; i++)
		{
			float x = radiusFromTarget * Mathf.Sin(angleDiff * i);
			float y = radiusFromTarget * Mathf.Cos(angleDiff * i);
			positionsAroundTarget.Add(new Vector3(x, 1, y));
		}
		positionIndex = 0;

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
			positionIndex = (positionIndex + 1) % (int)(positionsNum);
		}
	}

}

