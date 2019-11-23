using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHallways : MonoBehaviour
{

    public struct ReturnInfo
    {
        public Vector3 nextSpawnPoint;
        public float currentYRotation;
    }

    System.Random random = new System.Random();

    [SerializeField] GameObject[] wallTypes;

    int numberOfWallTypes;

    void Start()
    {
        numberOfWallTypes = wallTypes.Length;

        ReturnInfo start = new ReturnInfo();
        start.nextSpawnPoint = Vector3.zero;
        start.currentYRotation = 0f;


        // during create asylum generate random pieces at each step
        // (pieces = fork, hallway, room)
        // keep track of the last piece and if the current piece is the same
        // type as the last, stop a third of the same type from being chosen.
        BuildHallway(BuildHallway(start));
    }

    public ReturnInfo BuildHallway(ReturnInfo inputInfo)
    {
        ReturnInfo returnInfo = new ReturnInfo();

        Vector3 storeSpawnPoint1 = inputInfo.nextSpawnPoint;
        Vector3 newSpawnPoint1 = new Vector3();

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

            newWall1 = Instantiate(wallTypes[randIndex], storeSpawnPoint1,
                Quaternion.Euler(0, 90f - rotY, 0));

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

        //if (rotY == 180f)
        //{
        //    rotY = 0f;
        //}

        newWall1 = Instantiate(wallTypes[numberOfWallTypes-1],
            storeSpawnPoint1, Quaternion.Euler(0, 180f - rotY, 0));

        foreach (Transform child in newWall1.transform)
        {
            if (child.name == "SpawnPoint")
            {
                newSpawnPoint1 = child.position;
                break;
            }
        }



        returnInfo.nextSpawnPoint = new Vector3(newSpawnPoint1.x,
            0f, newSpawnPoint1.z);

        returnInfo.currentYRotation = inputInfo.currentYRotation - 90f;

        // parent everything to something....

        return returnInfo;
    }
}
