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
    private bool [] positionsAroundTarget;
    private uint nex_pos;
    private uint capacity;
    private float angleDiff;


    // Start is called before the first frame update
    void Start()
    {
        positionsAroundTarget = new bool[positionsNum];
        angleDiff = (2 * Mathf.PI) / positionsNum;
        for (int i = 0; i < positionsNum; i++)
        {
            positionsAroundTarget[i] = false;
        }
        nex_pos = 0;
        capacity = 0;
    }

    public (Vector3, uint index)? emplace()
	{
        // Suppose to be faster in this order 
        if(capacity >= positionsNum)
		{
            return null;
		}
        capacity++;

        //Search for the next open spot
        for(uint i = 0; i < positionsNum; i++)
		{
			if (!positionsAroundTarget[i])
			{
                positionsAroundTarget[i] = true;
                float x = radiusFromTarget * Mathf.Sin(angleDiff * i);
                float y = radiusFromTarget * Mathf.Cos(angleDiff * i);
                return (new Vector3(x, 1, y), i);
            }
		}

        // Should never happen
        throw new IndexOutOfRangeException("Building is full and the capacity was not updated correctly");
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

    void SetParent(EnemySpawner spawner)
	{
        // TODO: MAke sure this nly happens once?
        enemySpawner = spawner;
	}

}
