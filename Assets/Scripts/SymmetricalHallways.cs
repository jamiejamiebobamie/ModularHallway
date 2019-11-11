using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymmetricalHallways : MonoBehaviour
{

    GameObject floor, begin, exit;

    [SerializeField]
    GameObject[] wallTypes;

    Vector3 storeSpawnPoint1, storeSpawnPoint2;

    float randX;
    float randZ;

    System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        int numberOfWallTypes = wallTypes.Length;
        // generate a floor
        floor = GameObject.CreatePrimitive(PrimitiveType.Plane) as GameObject;
        floor.transform.localScale = new Vector3(100, 100, 100);

        //generate x coordinates for start and exit areas
        double randomDouble = random.NextDouble();
        double randVal = randomDouble * (double)floor.transform.localScale.x * 3
            - (double)floor.transform.localScale.x * 3;
        randX = (float)randVal - 200f;

        //generate z coordinates for start and exit areas
        randomDouble = random.NextDouble();
        randVal = randomDouble * (double)floor.transform.localScale.z * 5
            - (double)floor.transform.localScale.z * 5;

        double randDouble = random.NextDouble();
        float randFloatInRange = (float)randDouble * 2f - 1;
        randZ = (float)randVal * randFloatInRange;

        begin = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
        begin.transform.position = new Vector3(randX, 0, randZ);
        begin.name = "start";

        exit = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
        exit.transform.position = new Vector3(-randX, 0, randZ);
        exit.name = "exit";

        int count = 0;

        storeSpawnPoint1 = begin.transform.position;

        storeSpawnPoint2 = exit.transform.position;// + new Vector3(0,0 - 0.01198f);

        bool bend = false;
        int randInt;
        int countBends = 0; // do not allow more than 2 consecutive bends. the third bend becomes rotated in the other direction.
        float rotY = 0;
        //while (storeSpawnPoint1.x < 0)
        while (count < 10)

        {
            // "the bend" wall type is always the last in the array
            if (bend)
            {
                rotY += 270;
                randInt = random.Next(numberOfWallTypes - 1); // do not have two bends in a row
                bend = false;
            } else
            {
                //rotY = 0;
                randInt = random.Next(numberOfWallTypes); // the upper bound is exclusive
                if (randInt == numberOfWallTypes - 1)
                {
                    countBends++;
                    if (countBends == 3)
                    {

                    }
 
                    bend = true;
                }
            }

            GameObject newWall1 = Instantiate(wallTypes[randInt], storeSpawnPoint1, Quaternion.Euler(0, 90 - rotY, 0));
            GameObject newWall2 = Instantiate(wallTypes[randInt], storeSpawnPoint2, Quaternion.Euler(0, -90 - rotY, 0));

            Vector3 newSpawnPoint1 = new Vector3();
            foreach (Transform child in newWall1.transform)
            {
                if (child.name == "SpawnPoint")
                {
                    newSpawnPoint1 = child.position;
                    break;
                }
            }

            Vector3 newSpawnPoint2 = new Vector3();
            foreach (Transform child in newWall2.transform)
            {
                if (child.name == "SpawnPoint")
                {
                    newSpawnPoint2 = child.position;
                    break;
                }
            }

            //Vector3 newSpawnPoint = newWall.GetComponent<WallInfo>().spawnPoint;

            storeSpawnPoint1.x = newSpawnPoint1.x;
            storeSpawnPoint1.z = newSpawnPoint1.z;

            storeSpawnPoint2.x = newSpawnPoint2.x;
            storeSpawnPoint2.z = newSpawnPoint2.z;// - 0.01198f;

            count++;
    }
}
}
