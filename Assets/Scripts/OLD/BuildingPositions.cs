using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPositions : MonoBehaviour
{
    // Currently assuming the spots are all the same size and constant in number thru the building lifespan
    public float radiusFromTarget;
    public uint positionsNum;
    public EnemySpawner enemySpawner;

    private bool [] positionsAroundTarget;
    private uint nex_pos;
    private uint capacity;
    private float angleDiff;


    // Start is called before the first frame update
    void Start()
    {
        angleDiff = 360.0f / positionsNum;
        for (int i = 0; i < positionsNum; i++)
        {
            positionsAroundTarget[i] = false;
        }
        nex_pos = 0;
        capacity = 0;
    }

    public Vector3? emplace()
	{
        // Suppose to be faster in this order 
        if(capacity >= positionsNum)
		{
            return null;
		}
        capacity++;

        //Search for the next open spot
        for(int i = 0; i < positionsNum; i++)
		{
			if (!positionsAroundTarget[i])
			{
                float x = radiusFromTarget * Mathf.Sin(angleDiff * i);
                float y = radiusFromTarget * Mathf.Cos(angleDiff * i);
                return new Vector3(x, 1, y);
            }
		}

        // Should never happen
        throw new IndexOutOfRangeException("Building is full and the capacity was not updated correctly");
    }

    void remove(uint index)
	{
        if (!positionsAroundTarget[index])
		{
            throw new ArgumentException("Cannot remove an enemy from an empty index--location");
        }
        capacity--;
        positionsAroundTarget[index] = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
