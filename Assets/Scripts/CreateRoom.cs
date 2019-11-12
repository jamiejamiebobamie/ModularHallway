using System.Collections;
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
    [SerializeField]
    Vector3 origin; // need to be able to generate rooms around an offset. it is not working...

    void Start()
    {
        // just for testing
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //floor.transform.position = new Vector3(0, 0, 0);
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
                0f, horizontalBoundsOfFloor.z)
                + new Vector3(2.408143f * (i + 1), 0f, 0f);

            Instantiate(wallTypes[current_index],
                newLocation,
                Quaternion.Euler(0, -90, 0));

            newWall = Instantiate(wallTypes[current_index],
                newLocation, Quaternion.Euler(0, -90, 0));

            newWall.transform.parent = cieling.transform;

            //Instantiate(wallTypes[current_index],

            //    new Vector3(horizontalBoundsOfFloor.x,
            //    0f, -horizontalBoundsOfFloor.z)

            //    +new Vector3(2.408143f * i, 0f, 0f),

            //    //- new Vector3(0f,0f,newLocation.z + floor.transform.localScale.z), // need to send the new wall down the z axis to the other end of the floor/room.
            //    //-new Vector3(0f, 0f, horizontalBoundsOfFloor.z),

            //    Quaternion.Euler(0, 90, 0));
        }

        for (int i = 0; i < numOfVerticalWalls - length + 1; i++) // need to increment the total by total - total/percent of total
        {
            current_index = i % wallTypes.Length;

            newLocation = new Vector3(verticalBoundsOfFloor.x,
                0f, verticalBoundsOfFloor.z)
                + new Vector3(0, 0f, 2.408143f * i);

            Instantiate(wallTypes[current_index],
                newLocation,
                Quaternion.Euler(0, 0, 0));

            newWall = Instantiate(wallTypes[current_index],
                newLocation,
                Quaternion.Euler(0, 0, 0));

            newWall.transform.parent = cieling.transform;

            //Instantiate(wallTypes[current_index],

            //    new Vector3(0f,
            //    0f, -verticalBoundsOfFloor.z)

            //    + new Vector3(0f, 0f, -2.408143f * i),

            //    Quaternion.Euler(0, 180, 0));
            //Instantiate(wallTypes[current_index],
            //    new Vector3(verticalBoundsOfFloor.x,
            //    0f, verticalBoundsOfFloor.z)
            //    + new Vector3(0f, 0f, -2.408143f * i) - origin,
            //    Quaternion.Euler(0, 180, 0));
        }

        cieling.transform.Rotate(0f, 180f, 0f);
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
