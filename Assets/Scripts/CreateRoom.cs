﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    GameObject[] wallTypes;

    [SerializeField] [Range(1,4)]
    int numOfOpenings;

    [SerializeField]
    int numOfIslands;

    [SerializeField]
    int length;

    [SerializeField]
    int width;

    System.Random random = new System.Random();

    //2.408143 // size of wall

    // Start is called before the first frame update
    void Start()
    {
        // just for testing
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = new Vector3(0, 0, 0);
        floor.transform.localScale = new Vector3(width, 1, length);

        int numOfHorizontalWalls = 5 * width;
        int numOfVerticalWalls =  5 * length;

        Vector3 horizontalBoundsOfFloor = floor.transform.position - 5*floor.transform.localScale;
        Vector3 verticalBoundsOfFloor = floor.transform.position - 5 * floor.transform.localScale;

        for (int i = 0; i < numOfHorizontalWalls - width +1; i++) // there are a lot of excess walls the larger the number gets
        {
            int current_index = i % wallTypes.Length;
            Instantiate(wallTypes[current_index], new Vector3(horizontalBoundsOfFloor.x,0f, -horizontalBoundsOfFloor.z) + new Vector3(2.408143f * i ,0f,0f), Quaternion.Euler(0,90,0));
            Instantiate(wallTypes[current_index], new Vector3(-horizontalBoundsOfFloor.x,0f, horizontalBoundsOfFloor.z) + new Vector3(-2.408143f * i, 0f, 0f), Quaternion.Euler(0, -90, 0));
        }

        for (int i = 0; i < numOfVerticalWalls - length + 1; i++) // need to increment the total by total - total/percent of total
        {
            int current_index = i % wallTypes.Length;
            Instantiate(wallTypes[current_index], new Vector3(-verticalBoundsOfFloor.x, 0f, verticalBoundsOfFloor.z) + new Vector3(0, 0f, 2.408143f * (i+1)), Quaternion.Euler(0, 180, 0));
            Instantiate(wallTypes[current_index], new Vector3(verticalBoundsOfFloor.x, 0f, -verticalBoundsOfFloor.z) + new Vector3(0f, 0f, -2.408143f * (i+1)), Quaternion.Euler(0, 0, 0));
        }
        //float numOfWalls = (float)length % 2.408143f;
        //Debug.Log(numOfWalls);

        // number of islands
        if (length > 1 && width > 1)
        {
            int rand = random.Next(1, (length - 1) * (width - 1));
            Debug.Log(rand);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
