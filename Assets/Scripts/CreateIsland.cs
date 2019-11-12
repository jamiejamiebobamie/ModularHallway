using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIsland : MonoBehaviour
{
    [SerializeField]
    GameObject[] wallTypes;

    [SerializeField]
    Vector3 origin;

    [SerializeField] int length, width;

    System.Random random = new System.Random();

    float sizeOfWall = 2.408143f;

    void Start()
    {

        // just for testing
        GameObject cieling = GameObject.CreatePrimitive(PrimitiveType.Plane);

        cieling.transform.position = new Vector3(0, 3f, 0) + origin;

        cieling.transform.localScale = new Vector3(width, 1, length);
        cieling.transform.localRotation = Quaternion.Euler(180, 0, 0);

        int numOfHorizontalWalls = 5 * width;
        int numOfVerticalWalls = 5 * length;

        Vector3 horizontalBoundsOfFloor = cieling.transform.position - 5 * cieling.transform.localScale;
        Vector3 verticalBoundsOfFloor = cieling.transform.position - 5 * cieling.transform.localScale;

        GameObject newWall;
        Vector3 newLocation;
        Vector3 currentEulerRot;
        int current_index;

        for (int i = 0; i < numOfHorizontalWalls - width + 1; i++) // there are a lot of excess walls the larger the number gets
        {
            if (i == 0 || i == numOfHorizontalWalls - width)
            {
                current_index = 0; //use corner piece at the beginning and end
                if (i == numOfHorizontalWalls - width)
                    currentEulerRot = new Vector3(0f,-90f,0f);
                else
                    currentEulerRot = new Vector3(0f, 90f, 0f);
            }
            else
            {
                current_index = 1; //use wall piece
                currentEulerRot = new Vector3(0f, 90f, 0f);
            }

            newLocation = new Vector3(horizontalBoundsOfFloor.x,
                origin.y, horizontalBoundsOfFloor.z)
                + new Vector3(sizeOfWall * (i + 1), 0f, 0f);

            Instantiate(wallTypes[current_index],
                newLocation,
                Quaternion.Euler(currentEulerRot));

            newWall = Instantiate(wallTypes[current_index],
                newLocation, Quaternion.Euler(currentEulerRot));

            newWall.transform.parent = cieling.transform;
        }

        //for (int i = 0; i < numOfVerticalWalls - length + 1; i++) // need to increment the total by total - total/percent of total
        //{
        //    current_index = i % wallTypes.Length;

        //    newLocation = new Vector3(verticalBoundsOfFloor.x,
        //        origin.y, verticalBoundsOfFloor.z)
        //        + new Vector3(0, 0f, sizeOfWall * i);

        //    Instantiate(wallTypes[current_index],
        //        newLocation,
        //        Quaternion.Euler(0, 0, 0));

        //    newWall = Instantiate(wallTypes[current_index],
        //        newLocation,
        //        Quaternion.Euler(0, 0, 0));

        //    newWall.transform.parent = cieling.transform;
        //}

        cieling.transform.Rotate(0f, 180f, 0f);
        //float numOfWalls = (float)length % 2.408143f;
        //Debug.Log(numOfWalls);

        // number of islands
        if (length > 1 && width > 1)
        {
            int rand = random.Next(1, (length - 1) * (width - 1));
            if (rand > 10)
                rand = 10;
            Debug.Log(rand);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}

