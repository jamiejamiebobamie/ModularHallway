using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAsylum : MonoBehaviour
{
    System.Random random = new System.Random();
    // the order of the wallTypes matter:
    // Hallway_door, Hallway_window, bendDown, bendUp
    [SerializeField] GameObject[] wallTypes;
    [SerializeField] GameObject[] singleWalls;
    [SerializeField] GameObject fork, island;
    Vector3 storeSpawnPoint1, storeSpawnPoint2;
    int numberOfWallTypes, missingWall1, missingWall2;
    float sizeOfWall = 2.408143f;

    void Start()
    {
        numberOfWallTypes = wallTypes.Length;

        //Vector3 currentPosition = Room(Vector3.zero, 10, 10, false);

        Vector3 currentPosition = Hallway(Vector3.zero, 25);

        currentPosition = Fork(currentPosition);

        currentPosition = Hallway(currentPosition, 25);
    }



    Vector3 Room(Vector3 origin, int length, int width, bool deadEnd)
    {
        // the deadEnd bool indicates the room only has one entrance/exit.


        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = origin;
        floor.transform.localScale = new Vector3(width, 1, length);

        GameObject cieling = GameObject.CreatePrimitive(PrimitiveType.Plane);

        cieling.transform.position = new Vector3(0, 3f, 0) + origin;

        cieling.transform.localScale = new Vector3(width, 1, length);
        cieling.transform.localRotation = Quaternion.Euler(180, 0, 0);

        int numOfHorizontalWalls = 5 * width;
        int numOfVerticalWalls = 5 * length;

        // the number of walls does not scale well. upwards of 5 and there
        // are gaps at the corners. need a different math function below
        // to cover the increasing scale.
        if (length > 5 || width > 5)
        {
            numOfHorizontalWalls = numOfHorizontalWalls - width+2;
            numOfVerticalWalls = numOfVerticalWalls - length+2;
        } else
        {
            numOfHorizontalWalls = numOfHorizontalWalls - width+1;
            numOfVerticalWalls = numOfVerticalWalls - length+1;
        }

        Vector3 horizontalBoundsOfFloor = floor.transform.position
            - 5 * floor.transform.localScale;
        Vector3 verticalBoundsOfFloor = floor.transform.position
            - 5 * floor.transform.localScale;

        GameObject newWall;
        Vector3 newLocation;
        int current_index;

        missingWall1 = random.Next(1,numOfHorizontalWalls - width);

        if (!deadEnd)
        {
          missingWall2 = random.Next(1, numOfVerticalWalls - length);
        }

        for (int i = 0; i < numOfHorizontalWalls; i++)
        {


            current_index = i % singleWalls.Length;

            newLocation = new Vector3(horizontalBoundsOfFloor.x,
                origin.y, horizontalBoundsOfFloor.z)
                + new Vector3(sizeOfWall * (i + 1), 0f, 0f);

            Instantiate(singleWalls[current_index],
                newLocation,
                Quaternion.Euler(0, -90, 0));

            if (i != missingWall1)
            {
                newWall = Instantiate(singleWalls[current_index],
                    newLocation, Quaternion.Euler(0, -90, 0));

                newWall.transform.parent = cieling.transform;
            }

        }

        for (int i = 0; i < numOfVerticalWalls; i++)
        {

            current_index = i % singleWalls.Length;

            newLocation = new Vector3(verticalBoundsOfFloor.x,
                origin.y, verticalBoundsOfFloor.z)
                + new Vector3(0, 0f, sizeOfWall * i);

            Instantiate(singleWalls[current_index],
                newLocation,
                Quaternion.Euler(0, 0, 0));



            if (i == missingWall2 && !deadEnd)
            {
                continue;
            } else
            {
                newWall = Instantiate(singleWalls[current_index],
                    newLocation, Quaternion.Euler(0, 0, 0));
                newWall.transform.parent = cieling.transform;
            }

}

        cieling.transform.Rotate(0f, 180f, 0f);

        int rand = 0;
        // number of islands
        if (length > 1 && width > 1)
        {
            rand = random.Next(1, (length - 1) * (width - 1));
            if (rand > 10)
                rand = 10;
            Debug.Log(rand);
        }

        for (int i = 0; i < rand; i++)
        {
            double upperBounds = origin.x + (width * 5f - sizeOfWall * 3f);
            double lowerBounds = origin.x - (width * 5f - sizeOfWall * 3f);
            double rangeOfValues = upperBounds - lowerBounds;
            double randomDub = random.NextDouble();
            double randX = randomDub * rangeOfValues
                - System.Math.Abs(lowerBounds);

            upperBounds = origin.z + (length * 5f - sizeOfWall * 3f);
            lowerBounds = origin.z - (length * 5f - sizeOfWall * 3f);
            rangeOfValues = upperBounds - lowerBounds;
            randomDub = random.NextDouble();
            double randZ = randomDub * rangeOfValues
                - System.Math.Abs(lowerBounds);

            Vector3 location = new Vector3((float)randX,
                origin.y, (float)randZ);

            GameObject newIsland = Instantiate(island,
                location, Quaternion.identity);
            newIsland.transform.localScale = new Vector3(2f, 1f, 2f);//testing
        }

        return Vector3.zero;
    }

    Vector3 Fork(Vector3 spawnLocation)
    {
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
        int deadEnd = random.Next(0, 2);
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
        }
        else
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
            return storeSpawnPoint1;
        else
            return storeSpawnPoint2;

    }

    Vector3 Hallway(Vector3 start, int limit)
    {
        storeSpawnPoint1 = start;
        int count = 0;
        bool bend = false;
        int randInt;
        int countBends = 0;
        float rotY = 0;
        GameObject newWall1;
        Vector3 newSpawnPoint1 = new Vector3();
        bool endHallway = false;

        while (count < limit)
        {

            if (endHallway)
            {
                return storeSpawnPoint1;
            }

            if (countBends == 0)
            {
                rotY = 90;
            }

            // "the bend" wall type is always the last in the array
            if (bend)
            {
                // spawn room.

                rotY += 270;
                randInt = random.Next(numberOfWallTypes - 2); // do not have two bends in a row
                bend = false;
            }
            else
            {
                randInt = random.Next(numberOfWallTypes - 1); // the upper bound is exclusive
                if (randInt == numberOfWallTypes - 2)
                {
                    countBends++;
                    if (countBends == 3)
                    {
                        endHallway = true;
                        //rotY = 0;
                        countBends = 0;
                        randInt = 3; // use the BendUp, which is a bend in the opposite direction as the last bends.
                    }
                    bend = true;
                }
            }

            newWall1 = Instantiate(wallTypes[randInt], storeSpawnPoint1, Quaternion.Euler(0, 90 - rotY, 0));

            foreach (Transform child in newWall1.transform)
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
        return newSpawnPoint1;
    }
}


