using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPositions : MonoBehaviour
{
    // Currently assuming the spots are all the same size and constant in number thru the building lifespan
    public float radiusFromTarget;
    public uint positionsNum;
    
    private EnemySpawner enemySpawner;
    [SerializeField]
    private bool [] positionsAroundTarget;
    [SerializeField]
    private uint capacity;
    [SerializeField]
    private float angleDiff;

    public uint building_id;

    // Start is called before the first frame update
    void Awake()
    {
        positionsAroundTarget = new bool[positionsNum];
        angleDiff = (2 * Mathf.PI) / positionsNum;
        for (int i = 0; i < positionsNum; i++)
        {
            positionsAroundTarget[i] = false;
        }
        capacity = 0;
        building_id = 0;
    }

    public void SetId(uint id) { building_id = id; }

    public (Vector3, uint index)? emplace()
	{
        // Suppose to be faster in this order 
        if(capacity >= positionsNum)
		{
           return null;
		}

        //Search for the next open spot
        for(uint i = 0; i < positionsNum; i++)
		{
			if (!positionsAroundTarget[i])
			{
                positionsAroundTarget[i] = true;
                float x = radiusFromTarget * Mathf.Sin(angleDiff * i) + transform.position.x;
                float y = radiusFromTarget * Mathf.Cos(angleDiff * i) + transform.position.z;
                capacity++;
                return (new Vector3(x, 1, y), i);
            }
		}

        // Should never happen
        throw new IndexOutOfRangeException("Building is full and the capacity was not updated correctly");
        //return null;
    }

    public void remove(uint index)
	{
        if (!positionsAroundTarget[index])
		{
            throw new ArgumentException("Cannot remove an enemy from an empty index--location");
        }
        capacity--;
        positionsAroundTarget[index] = false;
    }

    public void SetParent(EnemySpawner spawner)
	{
        // TODO: MAke sure this nly happens once?
        enemySpawner = spawner;
	}

	private void OnDestroy()
	{
        if (enemySpawner != null)
        {
            // TODO: on game exit this gets to be null
            enemySpawner.RemoveBuilding(building_id);
        }
    }

}
