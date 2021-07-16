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


	//public List<BuildingPositions> buildings;
	// building_id, buiding_reference
	public Dictionary<uint, BuildingPositions> buildings;
	// Map the buiding and index in bulding position to a specific enemy
	// enemy_ref, building id(from the buildings dict) and the placing index 
	Dictionary<EnemyController, (uint, uint)> enemies_building_map;
	// Map the building to an enemies list
	// building id from buildings list, enemy ref
	Dictionary<uint, List<EnemyController>> building_enemies_map;

	// Enemies with current unassigned target
	[SerializeField]
	Queue<EnemyController> waiting_list_;

	// Removal event - used in order to avoid race conditions
	[SerializeField]
	Queue<EnemyController> remove_enemy_events;
	[SerializeField]
	Queue<uint> remove_building_events;


	public Vector3 AttackSlot()
	{
		return positionsAroundTarget[positionIndex];
	}

	public void AddBuilding(uint id, BuildingPositions buildingRef)
	{
		buildings.Add(id, buildingRef);
	}

	// Start is called before the first frame update
	void Start()
	{
		enemies_building_map = new Dictionary<EnemyController, (uint, uint)>();
		building_enemies_map = new Dictionary<uint, List<EnemyController>>();
		waiting_list_ = new Queue<EnemyController>();
		//buildings = new List<BuildingPositions>();

		remove_enemy_events = new Queue<EnemyController>();
		remove_building_events = new Queue<uint>();
		buildings = new Dictionary<uint, BuildingPositions>();
		buildings.Add(0, GameObject.Find("CastleParent").GetComponent<BuildingPositions>());
	}


	(uint, Vector3, uint)? FindTarget()
	{
		(Vector3, uint)? pos = null;
		int i = 0;
		foreach (var b in buildings)
		{
			// TODO: this is a temporary only fix..
			if (b.Value != null)
			{
				pos = b.Value.emplace();
				if (pos != null)
				{
					return (b.Key, pos.Value.Item1, pos.Value.Item2);
				}
			}
		}

		return null;
	}


	EnemyController SpawnEnemy()
	{
		float theta = Random.value * (2 * Mathf.PI);
		float x = spawnDistance * Mathf.Sin(theta);
		float y = spawnDistance * Mathf.Cos(theta);

		Vector3 spawn_position = new Vector3(x, 0.5f, y);

		Transform res = Instantiate(enemyRef, spawn_position, Quaternion.identity);
		
		return res.GetComponent<EnemyController>();
	}

	// Update is called once per frame
	void Update()
	{

		// Handle remove enemy evet
		while (remove_enemy_events.Count > 0)
		{
			var removed = remove_enemy_events.Dequeue();
			HandleRemoveEnemy(removed);

		}
		// Hanvlde remove building event
		while (remove_building_events.Count > 0)
		{
			var removed = remove_building_events.Dequeue();
			HandleRemoveBuilding(removed);
		}
		
		timeCounter += Time.deltaTime;
		if (timeCounter > timeout)
		{
			var target = FindTarget();
			if (target != null)
			{
				// Unpack tuple
				uint target_building_id = target.Value.Item1;
				Vector3 target__slot_pos = target.Value.Item2;
				uint target_building_index = target.Value.Item3;

				EnemyController enemy_res = null;
				if (waiting_list_.Count > 0)
				{ // Use the waiting enemy
					enemy_res = waiting_list_.Dequeue();
				}
				else
				{ // summon a new enemy
					enemy_res = SpawnEnemy();
				}
				enemy_res.SetTarget(
						buildings[target_building_id].transform.position,
						target__slot_pos,
						this);

				// Update the tracing "databases"
				if (!building_enemies_map.ContainsKey(target_building_id)){
					building_enemies_map[target_building_id] = new List<EnemyController>();
				}
				building_enemies_map[target_building_id].Add(enemy_res);

				enemies_building_map.Add(enemy_res,
					(target_building_id,
					target_building_index));

				timeCounter = 0;
			}
			// TODO: What happens if theres no place?
		}

	}

	public void RemoveEnemy(EnemyController caller)
	{
		remove_enemy_events.Enqueue(caller);
	}
	public void RemoveBuilding(uint id)
	{
		remove_building_events.Enqueue(id);
	}
	private void HandleRemoveEnemy(EnemyController caller)
	{
		uint building_id;
		uint pos;
		(building_id, pos) = enemies_building_map[caller];
		buildings[building_id].remove(pos);
	}

	private void HandleRemoveBuilding(uint building_id)
	{
		// Remove the building from the buildings list
		buildings.Remove(building_id);

		// Tell al of its enemies they are "target-less" and add them to the waiting list
		if (building_enemies_map.ContainsKey(building_id))
		{
			var enemies = building_enemies_map[building_id];
			building_enemies_map.Remove(building_id);
			foreach (var enemy in enemies)
			{
				if (enemy != null)
				{
					waiting_list_.Enqueue(enemy);
					enemies_building_map.Remove(enemy);
					enemy.target_less = true;
				}
			}
		}
	}



}

