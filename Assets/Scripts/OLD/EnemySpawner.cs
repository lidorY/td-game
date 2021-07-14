using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	public Transform enemyRef;
	public List<Transform> buildings;


	public Transform attakTarget;
	public uint positionsNum;
	public float radiusFromTarget;

	private List<Vector3> positionsAroundTarget;
	private int positionIndex = 0;

	public float spawnDistance;
	public float timeout;

	private float timeCounter;

	Dictionary<EnemyController, (BuildingPositions, uint)> enemies_building_map;
	Dictionary<BuildingPositions, List<EnemyController>> building_enemies_map;
	public Vector3 AttackSlot()
	{
		return positionsAroundTarget[positionIndex];
	}

	public void AddBuilding(Transform buildingRef)
	{
		buildings.Add(buildingRef);
	}

	// Start is called before the first frame update
	void Start()
	{
		enemies_building_map = new Dictionary<EnemyController, (BuildingPositions, uint)>();
		building_enemies_map = new Dictionary<BuildingPositions, List<EnemyController>>();
	}


	(Transform, Vector3, uint)? FindTarget()
	{
		(Vector3, uint)? pos = null;
		Transform building_ref = null;
		int i = 0;
		for (i = 0; i < buildings.Count; i++)
		{
			pos = buildings[i].GetComponent<BuildingPositions>().emplace();
			if (pos != null)
			{
				building_ref = buildings[i];
				return (buildings[i], pos.Value.Item1, pos.Value.Item2);
			}
		}

		return null;
	}

	// Update is called once per frame
	void Update()
	{
		timeCounter += Time.deltaTime;
		if (timeCounter > timeout)
		{
			var target = FindTarget();
			if (target != null)
			{
				float theta = Random.value * (2 * Mathf.PI);
				float x = spawnDistance * Mathf.Sin(theta);
				float y = spawnDistance * Mathf.Cos(theta);
				
				Vector3 spawn_position = new Vector3(x, 0.5f, y);

				Transform res = Instantiate(enemyRef, spawn_position, Quaternion.identity);
				res.GetComponent<EnemyController>().SetTarget(
					target.Value.Item1.position,
					target.Value.Item2,
					this);

				if (!building_enemies_map.ContainsKey(target.Value.Item1.GetComponent<BuildingPositions>()))
				{
					building_enemies_map[target.Value.Item1.GetComponent<BuildingPositions>()] = new List<EnemyController>();
				}
				building_enemies_map[target.Value.Item1.GetComponent<BuildingPositions>()].Add(res.GetComponent<EnemyController>());

				enemies_building_map.Add(res.GetComponent<EnemyController>(), (target.Value.Item1.GetComponent<BuildingPositions>(), target.Value.Item3));

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

	public void RemoveBuilding(BuildingPositions buiding)
	{
		var enemies = building_enemies_map[buiding];

		foreach(var enemy in enemies)
		{
			//var target = FindTarget();
			//enemy.SetTarget(target.Value.)
		}

	}
}

