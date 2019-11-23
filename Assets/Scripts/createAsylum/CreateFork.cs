using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFork : MonoBehaviour
{
    public struct ReturnInfo
    {
        public Vector3 nextSpawnPoint;
        public float currentYRotation;
    }

    [SerializeField]
    GameObject fork;

    [SerializeField]
    Vector3 origin;

    [SerializeField]
    GameObject[] wallTypes;

    Vector3 storeSpawnPoint1, storeSpawnPoint2;

    System.Random random = new System.Random();

    int numberOfWallTypes;

    void Start()
    {
        numberOfWallTypes = wallTypes.Length;
        ReturnInfo start = new ReturnInfo();

        start.nextSpawnPoint = origin;
        Fork(start);
    }

    ReturnInfo Fork(ReturnInfo inputInfo) {
        ReturnInfo returnInfo = new ReturnInfo();

        Vector3 spawnLocation = inputInfo.nextSpawnPoint;
        // spawn a fork at spawnLocation
        // and return the location that the path should continue down.

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
        } else
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

        // call room script to put an end on the dead end.

        if (deadEnd == 1)
            returnInfo.nextSpawnPoint = storeSpawnPoint1;
        else
            returnInfo.nextSpawnPoint = storeSpawnPoint2;

        return returnInfo;
    }
}
