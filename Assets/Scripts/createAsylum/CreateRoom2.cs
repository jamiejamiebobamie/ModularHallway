using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom2 : MonoBehaviour
{

    System.Random random = new System.Random();

    public struct ReturnInfo
    {
        public Vector3 nextSpawnPoint;
        public float currentYRotation;
    }

    // the order of the wallTypes matter:
    // Hallway_door, Hallway_window, bendDown, bendUp
    [SerializeField] GameObject[] wallTypes;
	[SerializeField] GameObject island, opening;
    [SerializeField] GameObject[] singleWalls;

    [SerializeField] bool deadEnd;

    [SerializeField] [Range(1, 10)] int length;
	[SerializeField] [Range(1, 10)] int width;

	float sizeOfWall = 2.408143f;

    [SerializeField]
    Vector3 origin;

    [SerializeField]
    float serializedRotationY;

    int numberOfWallTypes, missingWall1, missingWall2;

    GameObject spawnSphere2, spawnSphere;

    // Start is called before the first frame update
    void Start()
    {
        ReturnInfo start = new ReturnInfo();
        start.nextSpawnPoint = origin;
        start.currentYRotation = serializedRotationY;
        ReturnInfo returnInfo = Room(start);
    }

    ReturnInfo Room(ReturnInfo inputInfo)
	{

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

		missingWall1 = random.Next(1, numOfHorizontalWalls - width-1);

		if (!deadEnd)
		{
			missingWall2 = random.Next(1, numOfVerticalWalls - length-1);
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
				spawnRoom = newLocation; // going to have to parent everything
                                         // to the floor or cieling and create a method that shifts
                                         // the room so that this opening aligns with the last generated
                                         // floor piece before entering this method. (ie this location
                                         // is the entrance to the room)
                spawnSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spawnSphere.transform.position = spawnRoom;

                //GameObject spawnSphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), spawnRoom, Quaternion.identity);
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


            if (i == missingWall2 && !deadEnd)
			{
                spawnRoom = newLocation; // going to have to parent everything
                                         // to the floor or cieling and create a method that shifts
                                         // the room so that this opening aligns with the last generated
                                         // floor piece before entering this method. (ie this location
                                         // is the entrance to the room)
                //GameObject spawnSphere2 = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), spawnRoom, Quaternion.identity);
                spawnSphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spawnSphere2.transform.position = spawnRoom;
                spawnSphere2.transform.parent = cieling.transform;
                //// this is the exit opening.
                //            newReturnInfo.nextSpawnPoint = newLocation;
                //newReturnInfo.currentYRotation = 0f;

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
			//Debug.Log(rand);
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
			newIsland.transform.localScale = new Vector3(2f, 1f, 2f);//testing
            newIsland.transform.parent = floor.transform;

        }

        GameObject entranceAndRoomRoot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        entranceAndRoomRoot.transform.position = spawnSphere.transform.position;
        floor.transform.parent = entranceAndRoomRoot.transform;



        GameObject entrance = Instantiate(opening, entranceAndRoomRoot.transform.position,
        Quaternion.Euler(0f, 270f, 0f));
        entrance.transform.parent = entranceAndRoomRoot.transform;
        entrance.name = "entrance";

        entranceAndRoomRoot.transform.localEulerAngles = new Vector3(0f, rotationY, 0f);
        entranceAndRoomRoot.transform.position = origin;

        if (spawnSphere2 != null)
        {
            GameObject exit = Instantiate(opening, spawnSphere2.transform.position,
                 Quaternion.Euler(0f, 0f, 0f));
            exit.transform.parent = entranceAndRoomRoot.transform;
            exit.name = "exit";


            newReturnInfo.nextSpawnPoint = exit.transform.position;
            newReturnInfo.currentYRotation = 90f;
            Debug.Log(newReturnInfo.nextSpawnPoint); // not correct...
        }

        //if (spawnSphere2 != null)
        //{
        //    newReturnInfo.nextSpawnPoint = spawnSphere2.transform.position;
        //    newReturnInfo.currentYRotation = 90f;
        //    Debug.Log(newReturnInfo.nextSpawnPoint);
        //}

        return newReturnInfo;
	}
}
