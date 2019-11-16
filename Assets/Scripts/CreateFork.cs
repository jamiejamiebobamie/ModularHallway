﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFork : MonoBehaviour
{

    [SerializeField]
    GameObject fork;

    [SerializeField]
    GameObject[] wallTypes;

    Vector3 storeSpawnPoint1, storeSpawnPoint2;

    System.Random random = new System.Random();

    int numberOfWallTypes;

    void Start()
    {
        numberOfWallTypes = wallTypes.Length;
        Vector3 testLocation = Vector3.zero;
        Fork(testLocation);
    }

    void Fork(Vector3 spawnLocation)
    {
        // create a new Fork in the road.
        GameObject newFork = Instantiate(fork, spawnLocation, Quaternion.identity);

        // get the world position of each spawn point (two of them)
        foreach (Transform child in newFork.transform)
        {
            if (child.name == "SpawnPoint1")
                storeSpawnPoint1 = child.transform.position;
            else if (child.name == "SpawnPoint2")
                storeSpawnPoint2 = child.transform.position;
        }

        // determine which way is the deadend
        int deadEnd = random.Next(0,2);
        // determine how many walls are generated before the dead end.
        int numOfWallsUntilDeadEnd = random.Next(3, 10);

        int randInt;

        int count = 0;

        GameObject newWall;

        if (deadEnd == 1)
        {
            Debug.Log("right");

            while (count < numOfWallsUntilDeadEnd)
            {
                randInt = random.Next(numberOfWallTypes - 1); // the upper bound is exclusive
                newWall = Instantiate(wallTypes[randInt], storeSpawnPoint1, Quaternion.Euler(0, 0, 0));

                Vector3 newSpawnPoint1 = new Vector3();
                foreach (Transform child in newWall.transform)
                {
                    if (child.name == "SpawnPoint")
                    {
                        newSpawnPoint1 = child.position;
                        break;
                    }
                }

                storeSpawnPoint1.x = newSpawnPoint1.x;
                storeSpawnPoint1.z = newSpawnPoint1.z;

                count++;
            }
        } else if (deadEnd == 0)
        {
            Debug.Log("left");

            while (count < numOfWallsUntilDeadEnd)
            {
                randInt = random.Next(numberOfWallTypes - 1); // the upper bound is exclusive
                newWall = Instantiate(wallTypes[randInt], storeSpawnPoint2, Quaternion.Euler(0, 180, 0));

                Vector3 newSpawnPoint2 = new Vector3();
                foreach (Transform child in newWall.transform)
                {
                    if (child.name == "SpawnPoint")
                    {
                        newSpawnPoint2 = child.position;
                        break;
                    }
                }

                storeSpawnPoint2.x = newSpawnPoint2.x;
                storeSpawnPoint2.z = newSpawnPoint2.z;

                count++;
            }
        }
    }
}
