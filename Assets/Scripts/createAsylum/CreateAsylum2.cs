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
    [SerializeField] GameObject island, opening, fork, startCell, exitDoor;
    [SerializeField] Vector3 origin;

    float sizeOfWall = 2.408143f;
    int numberOfWallTypes, missingWall1, missingWall2;

    //[SerializeField]
    int length, width;

    Vector3 storeSpawnPoint1, storeSpawnPoint2;

    void Start()
    {
        numberOfWallTypes = wallTypes.Length;

        ReturnInfo start = new ReturnInfo();
        start.nextSpawnPoint = origin;
        start.currentYRotation = 0f;

        Instantiate(startCell, start.nextSpawnPoint,
            Quaternion.Euler(0f, start.currentYRotation, 0f));

        //ReturnInfo spawnExit = Room(Room(start));
        ReturnInfo spawnExit = Fork(Hallway(start));

        Instantiate(exitDoor, spawnExit.nextSpawnPoint,
            Quaternion.Euler(0f, spawnExit.currentYRotation-90f, 0f));
    }

    ReturnInfo Room(ReturnInfo inputInfo)
    {
        ReturnInfo newReturnInfo = new ReturnInfo();
        Vector3 spawnRoom, exitPosition = Vector3.zero;
        Vector3 entrancePosition = Vector3.zero;

        // generate a random length and width
        // between 1 and 9 (inclusive)
        length = random.Next(1, 10);
        width = random.Next(1, 10);

        // spawn the floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = inputInfo.nextSpawnPoint;
        floor.transform.localScale = new Vector3(width, 1, length);

        // spawn the cieling
        GameObject cieling = GameObject.CreatePrimitive(PrimitiveType.Plane);
        cieling.transform.position = new Vector3(0, 3f, 0)
            + floor.transform.position;
        cieling.transform.localScale = new Vector3(width, 1, length);
        cieling.transform.localRotation = Quaternion.Euler(180, 0, 0);

        // determine the number of walls needed to line the room
        // horizontal = along the x-axis
        // vertical = along the z-axis
        int numOfHorizontalWalls = 5 * width - width + 2;
        int numOfVerticalWalls = 5 * length - length + 2;

        Vector3 spawnWallStartPosition = floor.transform.position
            - 5 * floor.transform.localScale;

        GameObject newWall;
        Vector3 newLocation;

        // index into the singleWalls Gameobject array
        int current_index;

        // determine which wall will be the entrance opening
        missingWall1 = random.Next(1, numOfHorizontalWalls - width - 1);

        // determine which wall will be the exit opening
            // if the room is not a dead end.
        if (!inputInfo.deadEnd)
            missingWall2 = random.Next(1, numOfVerticalWalls - length - 1);

        // generate walls that run along the x axis of the room.
        for (int i = 0; i < numOfHorizontalWalls; i++)
        {
            // alternate wall types
            current_index = i % singleWalls.Length;

            newLocation = new Vector3(spawnWallStartPosition.x,
                inputInfo.nextSpawnPoint.y, spawnWallStartPosition.z)
                + new Vector3(sizeOfWall * (i + 1), 0f, 0f);

            // generate two sets of walls. 
            newWall = Instantiate(singleWalls[current_index],
                newLocation,
                Quaternion.Euler(0f, -90f, 0));

            // one parented to the floor.
            newWall.transform.parent = floor.transform;

            if (i != missingWall1)
            {
                newWall = Instantiate(singleWalls[current_index],
                    newLocation, Quaternion.Euler(0f, -90f, 0f));

                // the other parented to the cieling.
                newWall.transform.parent = cieling.transform;
            }
            else
                entrancePosition = newLocation; // this is the entrance opening.
        }

        // generate walls that run along the z axis of the room.
        for (int i = 0; i < numOfVerticalWalls; i++)
        {

            current_index = i % singleWalls.Length;

            newLocation = new Vector3(spawnWallStartPosition.x,
                inputInfo.nextSpawnPoint.y, spawnWallStartPosition.z)
                + new Vector3(0f, 0f, sizeOfWall * i);

            newWall = Instantiate(singleWalls[current_index],
                newLocation,
                Quaternion.Euler(0f, 0f, 0f));
            newWall.transform.parent = floor.transform;

            if (i == missingWall2 && !inputInfo.deadEnd)
                exitPosition = newLocation; // this is the exit opening.
            else
            {
                newWall = Instantiate(singleWalls[current_index],
                    newLocation, Quaternion.Euler(0, 0, 0));
                newWall.transform.parent = cieling.transform;
            }

        }

        // rotate the cieling and the two sets of walls that are parented to it.
        cieling.transform.Rotate(0f, 180f, 0f);

        // parent the rotated cieling to the floor.
        cieling.transform.parent = floor.transform;

        int rand = 0;

        // generate islands of rooms within the room
        // do not generate islands if the length or width == 1 (too small)
        if (length > 1 && width > 1)
        {
            rand = random.Next(1, (length - 1) * (width - 1));
            if (rand > 10)
                rand = 10;
        }

        // for each island, determine its location.
            // leave the areas along the walls free of islands by determining
            // "bounds".
        for (int i = 0; i < rand; i++)
        {
            double upperBounds = inputInfo.nextSpawnPoint.x
                + (width * 5f - sizeOfWall * 3f);
            double lowerBounds = inputInfo.nextSpawnPoint.x
                - (width * 5f - sizeOfWall * 3f);
            double rangeOfValues = upperBounds - lowerBounds;
            double randomDub = random.NextDouble();

            double randX = randomDub * rangeOfValues
                - System.Math.Abs(lowerBounds);

            upperBounds = inputInfo.nextSpawnPoint.z
                + (length * 5f - sizeOfWall * 3f);
            lowerBounds = inputInfo.nextSpawnPoint.z
                - (length * 5f - sizeOfWall * 3f);
            rangeOfValues = upperBounds - lowerBounds;
            randomDub = random.NextDouble();

            double randZ = randomDub * rangeOfValues
                - System.Math.Abs(lowerBounds);

            Vector3 location = new Vector3((float)randX,
                inputInfo.nextSpawnPoint.y, (float)randZ);

            GameObject newIsland = Instantiate(island,
                location, Quaternion.identity);

            newIsland.transform.localScale =
                new Vector3(20f * width, 100f, 20f*width);

            newIsland.transform.parent = floor.transform;
        }

        GameObject roomRoot = new GameObject();

        roomRoot.transform.position = entrancePosition;
        floor.transform.parent = roomRoot.transform;

        GameObject entranceToRoomPrefab = Instantiate(opening,
            entrancePosition,
        Quaternion.Euler(0f, 270f, 0f));

        entranceToRoomPrefab.transform.parent = roomRoot.transform;
        entranceToRoomPrefab.name = "entrance";

        roomRoot.transform.localEulerAngles = new Vector3(0f,
            inputInfo.currentYRotation, 0f);

        roomRoot.transform.position = new Vector3 (inputInfo.nextSpawnPoint.x
            - sizeOfWall, inputInfo.nextSpawnPoint.y, inputInfo.nextSpawnPoint.z);

        if (exitPosition != Vector3.zero)
        {
            GameObject exit = Instantiate(opening,
                exitPosition,
                 Quaternion.Euler(0f, inputInfo.currentYRotation, 0f));

            exit.transform.parent = roomRoot.transform;
            exit.name = "exit";

            newReturnInfo.nextSpawnPoint = exitPosition;
            newReturnInfo.currentYRotation = inputInfo.currentYRotation - 90f;
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
        float rotY = inputInfo.currentYRotation;// +90f;

        GameObject newWall1;
        GameObject parent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        parent.transform.position = storeSpawnPoint;

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
            newWall1.transform.parent = parent.transform;


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
        newWall1.transform.parent = parent.transform;

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

        returnInfo.currentYRotation = inputInfo.currentYRotation;// + 90f;

        parent.transform.rotation = Quaternion.Euler(0f,
            inputInfo.currentYRotation, 0f);

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
                newWall.transform.parent = newFork.transform;


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
                newWall.transform.parent = newFork.transform;

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

        newFork.transform.rotation = Quaternion.Euler(0f, inputInfo.currentYRotation - 90f, 0f);

        // call Room() method to put an end on the dead end.
        ReturnInfo spawnRoom = new ReturnInfo();
        spawnRoom.deadEnd = true;

        if (deadEndBool == 1)
        {                // need the spawn point to be on the other side of the opening
                         // "sizeOfWall"+ sizeOfWall+50f
            spawnRoom.nextSpawnPoint = storeSpawnPoint2;//new Vector3 (storeSpawnPoint2.x
                //- sizeOfWall, storeSpawnPoint2.y, storeSpawnPoint2.z);
            spawnRoom.currentYRotation = 0f+ inputInfo.currentYRotation;

            returnInfo.currentYRotation = 180f + inputInfo.currentYRotation;
            returnInfo.nextSpawnPoint = storeSpawnPoint1;
        }
        else
        {
            //spawnRoom.nextSpawnPoint = storeSpawnPoint1;
            spawnRoom.nextSpawnPoint = storeSpawnPoint1;// new Vector3(storeSpawnPoint1.x - sizeOfWall,
                //storeSpawnPoint1.y, storeSpawnPoint1.z);
            spawnRoom.currentYRotation = 180f+ inputInfo.currentYRotation;

            returnInfo.currentYRotation = 0f + inputInfo.currentYRotation;
            returnInfo.nextSpawnPoint = storeSpawnPoint2;
 
        }
        Debug.Log(spawnRoom.nextSpawnPoint);

        //Room(spawnRoom);
        return returnInfo;
    }
}
