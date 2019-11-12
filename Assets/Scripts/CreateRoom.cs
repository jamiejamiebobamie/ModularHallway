using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    GameObject[] wallTypes;

    [SerializeField] [Range(1,4)]
    int numOfOpenings;

    //[SerializeField]
    int numOfIslands;

    [SerializeField] [Range(1, 10)]
    int length;

    [SerializeField] [Range(1, 10)]
    int width;

    System.Random random = new System.Random();

    float sizeOfWall = 2.408143f;

    // Start is called before the first frame update
    [SerializeField]
    Vector3 origin;

    void Start()
    {
        // just for testing
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = origin;
        floor.transform.localScale = new Vector3(width, 1, length);

        // just for testing
        GameObject cieling = GameObject.CreatePrimitive(PrimitiveType.Plane);

        cieling.transform.position = new Vector3(0, 3f, 0) + origin;

        cieling.transform.localScale = new Vector3(width, 1, length);
        cieling.transform.localRotation = Quaternion.Euler(180, 0, 0);

        int numOfHorizontalWalls = 5 * width;
        int numOfVerticalWalls =  5 * length;

        Vector3 horizontalBoundsOfFloor = floor.transform.position - 5 * floor.transform.localScale;
        Vector3 verticalBoundsOfFloor = floor.transform.position - 5 * floor.transform.localScale;

        GameObject newWall;
        Vector3 newLocation;
        int current_index;

        for (int i = 0; i < numOfHorizontalWalls - width +1; i++) // there are a lot of excess walls the larger the number gets
        {
            current_index = i % wallTypes.Length;

            newLocation = new Vector3(horizontalBoundsOfFloor.x,
                origin.y, horizontalBoundsOfFloor.z)
                + new Vector3(sizeOfWall * (i + 1), 0f, 0f);

            Instantiate(wallTypes[current_index],
                newLocation,
                Quaternion.Euler(0, -90, 0));

            newWall = Instantiate(wallTypes[current_index],
                newLocation, Quaternion.Euler(0, -90, 0));

            newWall.transform.parent = cieling.transform;
        }

        for (int i = 0; i < numOfVerticalWalls - length + 1; i++) // need to increment the total by total - total/percent of total
        {
            current_index = i % wallTypes.Length;

            newLocation = new Vector3(verticalBoundsOfFloor.x,
                origin.y, verticalBoundsOfFloor.z)
                + new Vector3(0, 0f, sizeOfWall * i);

            Instantiate(wallTypes[current_index],
                newLocation,
                Quaternion.Euler(0, 0, 0));

            newWall = Instantiate(wallTypes[current_index],
                newLocation,
                Quaternion.Euler(0, 0, 0));

            newWall.transform.parent = cieling.transform;

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
           GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            double upperBounds = origin.x + (width * 5f - sizeOfWall * 3f);
            double lowerBounds = origin.x - (width * 5f - sizeOfWall * 3f);
            double rangeOfValues = upperBounds - lowerBounds;
            double randomDub = random.NextDouble();
            double randX = randomDub * rangeOfValues - System.Math.Abs(lowerBounds);

            upperBounds = origin.z + (length * 5f - sizeOfWall * 3f);
            lowerBounds = origin.z - (length * 5f - sizeOfWall * 3f);
            rangeOfValues = upperBounds - lowerBounds;
            randomDub = random.NextDouble();
            double randZ = randomDub * rangeOfValues - System.Math.Abs(lowerBounds);

            Vector3 location = new Vector3((float)randX, origin.y,(float)randZ);
            Debug.Log(location);

            newCube.transform.localScale = new Vector3(width*3- 2.408143f*2, 0f,length*3- 2.408143f*2); // testing
            newCube.transform.position = location;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
