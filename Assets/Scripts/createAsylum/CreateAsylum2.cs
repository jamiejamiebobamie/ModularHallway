using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAsylum2 : MonoBehaviour
{

    System.Random random = new System.Random();

    public struct ReturnInfo
    {
        public Vector3 nextSpawnPoint;
        public float currentYRotation;
        public bool deadEnd;
    }

    // the order of the wallTypes matter:
    // Hallway_door, Hallway_window, bendDown, bendUp
    [SerializeField] GameObject[] wallTypes, singleWalls;
    [SerializeField] GameObject island, opening, fork;
    [SerializeField] Vector3 origin;

    float sizeOfWall = 2.408143f;
    int numberOfWallTypes, missingWall1, missingWall2;

    [SerializeField] int length, width;

    GameObject spawnSphere2, spawnSphere; // just for testing Room...
                                         // ...will be empty object.

    Vector3 storeSpawnPoint1, storeSpawnPoint2;

    void Start()
    {
        numberOfWallTypes = wallTypes.Length;

        ReturnInfo start = new ReturnInfo();
        start.nextSpawnPoint = origin;
        start.currentYRotation = 0f;


        //Room(start);
        Fork(start);

    }

    ReturnInfo Room(ReturnInfo inputInfo)
    {
        //length = random.Next(2, 10);
        //width = random.Next(2, 10);

        Vector3 spawnPoint = inputInfo.nextSpawnPoint;
        float rotationY = inputInfo.currentYRotation;

        // the deadEnd bool indicates the room only has one entrance/exit.

        ReturnInfo newReturnInfo = new ReturnInfo();

        Vector3 spawnRoom = Vector3.zero;

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = spawnPoint;
        floor.transform.localScale = new Vector3(width, 1, length);

        GameObject cieling = GameObject.CreatePrimitive(PrimitiveType.Plane);

        cieling.transform.position = new Vector3(0, 3f, 0) + spawnPoint;

        cieling.transform.localScale = new Vector3(width, 1, length);
        cieling.transform.localRotation = Quaternion.Euler(180, 0, 0);

        int numOfHorizontalWalls = 5 * width;
        int numOfVerticalWalls = 5 * length;

        // the number of walls does not scale well. upwards of 5 and there
        // are gaps at the corners. need a different math function below
        // to cover the increasing scale.

        if (length > 5 || width > 5)
        {
            numOfHorizontalWalls = numOfHorizontalWalls - width + 2;
            numOfVerticalWalls = numOfVerticalWalls - length + 2;
        }
        else
        {
            numOfHorizontalWalls = numOfHorizontalWalls - width + 1;
            numOfVerticalWalls = numOfVerticalWalls - length + 1;
        }

        Vector3 horizontalBoundsOfFloor = floor.transform.position
            - 5 * floor.transform.localScale;
        Vector3 verticalBoundsOfFloor = floor.transform.position
            - 5 * floor.transform.localScale;

        GameObject newWall;
        Vector3 newLocation;
        int current_index;

        missingWall1 = random.Next(1, numOfHorizontalWalls - width - 1);

        if (!inputInfo.deadEnd)
        {
            missingWall2 = random.Next(1, numOfVerticalWalls - length - 1);
        }

        for (int i = 0; i < numOfHorizontalWalls; i++)
        {
            current_index = i % singleWalls.Length;

            newLocation = new Vector3(horizontalBoundsOfFloor.x,
                spawnPoint.y, horizontalBoundsOfFloor.z)
                + new Vector3(sizeOfWall * (i + 1), 0f, 0f);

            newWall = Instantiate(singleWalls[current_index],
                newLocation,
                Quaternion.Euler(0, -90, 0));
            newWall.transform.parent = floor.transform;


            if (i != missingWall1)
            {
                newWall = Instantiate(singleWalls[current_index],
                    newLocation, Quaternion.Euler(0, -90, 0));

                newWall.transform.parent = cieling.transform;
            }
            else
            {
 
                   spawnRoom = newLocation;
                // this is the entrance opening.

                spawnSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                spawnSphere.transform.position = new Vector3(spawnRoom.x,
                    spawnRoom.y, spawnRoom.z);

                spawnSphere.transform.parent = cieling.transform;

            }

        }

        for (int i = 0; i < numOfVerticalWalls; i++)
        {

            current_index = i % singleWalls.Length;

            newLocation = new Vector3(verticalBoundsOfFloor.x,
                spawnPoint.y, verticalBoundsOfFloor.z)
                + new Vector3(0, 0f, sizeOfWall * i);

            newWall = Instantiate(singleWalls[current_index],
                newLocation,
                Quaternion.Euler(0, 0, 0));
            newWall.transform.parent = floor.transform;


            if (i == missingWall2 && !inputInfo.deadEnd)
            {
                spawnRoom = newLocation;
                spawnSphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spawnSphere2.transform.position = spawnRoom;
                spawnSphere2.transform.parent = cieling.transform;
                // this is the exit opening.
            }
            else
            {
                newWall = Instantiate(singleWalls[current_index],
                    newLocation, Quaternion.Euler(0, 0, 0));
                newWall.transform.parent = cieling.transform;
            }

        }

        cieling.transform.Rotate(0f, 180f, 0f);
        cieling.transform.parent = floor.transform;

        int rand = 0;

        // number of islands
        if (length > 1 && width > 1)
        {
            rand = random.Next(1, (length - 1) * (width - 1));
            if (rand > 10)
                rand = 10;
        }

        for (int i = 0; i < rand; i++)
        {
            double upperBounds = spawnPoint.x + (width * 5f - sizeOfWall * 3f);
            double lowerBounds = spawnPoint.x - (width * 5f - sizeOfWall * 3f);
            double rangeOfValues = upperBounds - lowerBounds;
            double randomDub = random.NextDouble();
            double randX = randomDub * rangeOfValues
                - System.Math.Abs(lowerBounds);

            upperBounds = spawnPoint.z + (length * 5f - sizeOfWall * 3f);
            lowerBounds = spawnPoint.z - (length * 5f - sizeOfWall * 3f);
            rangeOfValues = upperBounds - lowerBounds;
            randomDub = random.NextDouble();
            double randZ = randomDub * rangeOfValues
                - System.Math.Abs(lowerBounds);

            Vector3 location = new Vector3((float)randX,
                spawnPoint.y, (float)randZ);

            GameObject newIsland = Instantiate(island,
                location, Quaternion.identity);
            newIsland.transform.localScale =
                new Vector3(2f, 1f, 2f); // testing...
            newIsland.transform.parent = floor.transform;
        }

        GameObject entranceAndRoomRoot =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);
        entranceAndRoomRoot.transform.position =
            spawnSphere.transform.position;
        floor.transform.parent = entranceAndRoomRoot.transform;

        GameObject entrance = Instantiate(opening,
            entranceAndRoomRoot.transform.position,
        Quaternion.Euler(0f, 270f, 0f));
        entrance.transform.parent = entranceAndRoomRoot.transform;
        entrance.name = "entrance";

        entranceAndRoomRoot.transform.localEulerAngles = new Vector3(0f,
            rotationY, 0f);
        entranceAndRoomRoot.transform.position = inputInfo.nextSpawnPoint;

        if (spawnSphere2 != null)
        {
            GameObject exit = Instantiate(opening,
                spawnSphere2.transform.position,
                 Quaternion.Euler(0f, 0f, 0f));
            exit.transform.parent = entranceAndRoomRoot.transform;
            exit.name = "exit";


            newReturnInfo.nextSpawnPoint = exit.transform.position;
            newReturnInfo.currentYRotation = 90f;
            Debug.Log(newReturnInfo.nextSpawnPoint); // not correct...
        }

        return newReturnInfo;
    }


    public ReturnInfo Hallway(ReturnInfo inputInfo)
    {
        ReturnInfo returnInfo = new ReturnInfo();

        Vector3 storeSpawnPoint = inputInfo.nextSpawnPoint;
        Vector3 newSpawnPoint = new Vector3();

        int count = 0;
        bool bend = false;

        int randIndex;
        int countBends = 0;
        float rotY = inputInfo.currentYRotation;

        GameObject newWall1;

        while (countBends < 2)

        {
            if (bend)
            {
                rotY += 270;

                // do not have two bends in a row
                randIndex = random.Next(numberOfWallTypes - 2);
                bend = false;
            }
            else
            {
                randIndex = random.Next(numberOfWallTypes - 1);

                if (randIndex == numberOfWallTypes - 2)
                {
                    countBends++;
                    bend = true;
                }
            }

            newWall1 = Instantiate(wallTypes[randIndex], storeSpawnPoint,
                Quaternion.Euler(0, 90f - rotY, 0));

            foreach (Transform child in newWall1.transform)
            {
                if (child.name == "SpawnPoint")
                {
                    newSpawnPoint = child.position;
                    break;
                }
            }

            storeSpawnPoint.x = newSpawnPoint.x;
            storeSpawnPoint.z = newSpawnPoint.z;

            count++;
        }

        newWall1 = Instantiate(wallTypes[numberOfWallTypes - 1],
            storeSpawnPoint, Quaternion.Euler(0, 180f - rotY, 0));

        foreach (Transform child in newWall1.transform)
        {
            if (child.name == "SpawnPoint")
            {
                newSpawnPoint = child.position;
                break;
            }
        }

        returnInfo.nextSpawnPoint = new Vector3(newSpawnPoint.x,
            0f, newSpawnPoint.z);

        returnInfo.currentYRotation = inputInfo.currentYRotation - 90f;

        // parent everything to something....

        return returnInfo;
    }

    ReturnInfo Fork(ReturnInfo inputInfo)
    {
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
        int deadEndBool = random.Next(0, 2);
        // determine how many walls are generated before the dead end.
        int numOfWallsUntilDeadEnd = random.Next(3, 10);

        int randInt;

        int count = 0;

        GameObject newWall;

        if (deadEndBool == 1)
        {
            Debug.Log("right");

            while (count < numOfWallsUntilDeadEnd)
            {
                randInt = random.Next(numberOfWallTypes - 2); // the upper bound is exclusive
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
        }
        else
        {
            Debug.Log("left");

            while (count < numOfWallsUntilDeadEnd)
            {
                randInt = random.Next(numberOfWallTypes - 2); // the upper bound is exclusive
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

        // call Room() method to put an end on the dead end.
        ReturnInfo spawnRoom = new ReturnInfo();
        spawnRoom.deadEnd = true;

        if (deadEndBool == 1)
        {                // need the spawn point to be on the other side of the opening
                         // "sizeOfWall"+ sizeOfWall+50f
            spawnRoom.nextSpawnPoint = storeSpawnPoint2;//new Vector3 (storeSpawnPoint2.x
                //- sizeOfWall, storeSpawnPoint2.y, storeSpawnPoint2.z);
            spawnRoom.currentYRotation = 0f;
            returnInfo.nextSpawnPoint = storeSpawnPoint1;
        }
        else
        {
            //spawnRoom.nextSpawnPoint = storeSpawnPoint1;
            spawnRoom.nextSpawnPoint = storeSpawnPoint1;// new Vector3(storeSpawnPoint1.x - sizeOfWall,
                //storeSpawnPoint1.y, storeSpawnPoint1.z);
            spawnRoom.currentYRotation = 180f;
            returnInfo.nextSpawnPoint = storeSpawnPoint2;
        }
        Debug.Log(spawnRoom.nextSpawnPoint);
        Room(spawnRoom);
        return returnInfo;
    }
}
