using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{




	public Transform enemyRef;
	public Transform [] buildings;


	public Transform attakTarget;
	public uint positionsNum;
	public float radiusFromTarget;

	private List<Vector3> positionsAroundTarget;
	private int positionIndex = 0;

	public float spawnDistance;
	public float timeout;

	private float timeCounter;

	Dictionary<EnemyController, (BuildingPositions, uint)> enemies_building_map;
	public Vector3 AttackSlot()
	{
		return positionsAroundTarget[positionIndex];
	}


	// Start is called before the first frame update
	void Start()
	{
		enemies_building_map = new Dictionary<EnemyController, (BuildingPositions, uint)>();
	}

	// Update is called once per frame
	void Update()
	{
		timeCounter += Time.deltaTime;
		if (timeCounter > timeout)
		{
			(Vector3, uint)? pos = buildings[0].GetComponent<BuildingPositions>().emplace();
			if(pos != null)
			{
				float theta = Random.value * (2 * Mathf.PI);
				float x = spawnDistance * Mathf.Sin(theta);
				float y = spawnDistance * Mathf.Cos(theta);
				
				Vector3 spawn_position = new Vector3(x, 0.5f, y);

				Transform res = Instantiate(enemyRef, spawn_position, Quaternion.identity);
				res.GetComponent<EnemyController>().SetTarget(
					buildings[0].position, 
					pos.Value.Item1,
					this);

				enemies_building_map.Add(res.GetComponent<EnemyController>(), (buildings[0].GetComponent<BuildingPositions>(), pos.Value.Item2));
				timeCounter = 0;
			}
			// TODO: What happens if theres no place?
		}
	}

	public void RemoveEnemy(EnemyController caller)
	{
		var r = enemies_building_map[caller];
		r.Item1.remove(r.Item2);
	}

}

