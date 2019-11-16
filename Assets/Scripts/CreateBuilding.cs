using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuilding : MonoBehaviour
{

    public int limit;

    [SerializeField]
    GameObject[] wallTypes;

    System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        if (limit == 0)
        {
            limit = 5;
        }
        int numberOfWallTypes = wallTypes.Length;
        Build(numberOfWallTypes, limit);
    }

    void Build(int numberOfWallTypes, int limit)
    {
        int count = 0;
        bool bend = false;
        int randInt;
        int countBends = 0; // do not allow more than 2 consecutive bends. the third bend becomes rotated in the other direction.
        float rotY = 0;
        GameObject newWall1;
        GameObject newWall2 = gameObject;

        Vector3 storeSpawnPoint1 = Vector3.zero;
        Vector3 storeSpawnPoint2 = new Vector3(0f,0f, -0.01198f);


        while (count < limit)

        {
            if (countBends == 0)
            {
                rotY = -90;
            }

            // "the bend" wall type is always the last in the array
            if (bend)
            {
                rotY += 270;
                randInt = random.Next(numberOfWallTypes - 2); // do not have two bends in a row
                bend = false;
            }
            else
            {
                randInt = random.Next(numberOfWallTypes - 1); // the upper bound is exclusive
                Debug.Log(randInt);
                if (randInt == numberOfWallTypes - 2)
                {
                    countBends++;
                    if (countBends == 3)
                    {
                        countBends = 0;
                        randInt = 3; // use the BendUp, which is a bend in the opposite direction as the last bends.
                    }
                    bend = true;
                }
            }

            newWall1 = Instantiate(wallTypes[randInt], storeSpawnPoint1, Quaternion.Euler(0, 90 - rotY, 0));
            newWall2 = Instantiate(wallTypes[randInt], storeSpawnPoint2, Quaternion.Euler(0, -90 - rotY, 0));

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

            storeSpawnPoint1.x = newSpawnPoint1.x;
            storeSpawnPoint1.z = newSpawnPoint1.z;

            storeSpawnPoint2.x = newSpawnPoint2.x;
            storeSpawnPoint2.z = newSpawnPoint2.z;// - 0.01198f;


            count++;
        }
    }

    void Update()
    {
        
    }
}
