using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructHallway : MonoBehaviour
{

    public GameObject floor;

    int numberOfWallTypes = 1;

    private GameObject[] wall_types = new GameObject[1]; // 5
    private Vector3 storeSpawnPoint = Vector3.zero;
    private float storeRotY;

    int count;
    System.Random random = new System.Random();

    enum WallType
    {
        Window,
        Wall,
        Concave,
        Convex,
        Door,
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numberOfWallTypes; i++)
        {
            wall_types[i] = Resources.Load<GameObject>("wall_types/prefabs/asylum_walls"+i);
        }
        float width = floor.transform.localScale.x;
        float height = floor.transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("SpawnWalls", 3);
    }

    void SpawnWalls()
    {
        //while (count < 50)
        //{
        //    int randInt = random.Next(numberOfWallTypes);
        //    //Debug.Log(WallType.Window);

        //    Quaternion rot = Quaternion.Euler(0, storeRotY, 0);

        //    GameObject newWall = Instantiate(wall_types[randInt], storeSpawnPoint, rot);

        //    Vector3 newSpawnPoint = newWall.GetComponent<WallInfo>().spawnPoint;

        //    storeSpawnPoint.x += newSpawnPoint.x;
        //    storeSpawnPoint.z += newSpawnPoint.z;
        //    //if (randInt == 2)
        //    //    storeRotY += 90;
        //    //if (randInt == 3)
        //    //    storeRotY += 180;
        //    count++;
        //}
    }
}
