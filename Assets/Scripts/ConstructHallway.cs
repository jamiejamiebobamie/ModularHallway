using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructHallway : MonoBehaviour
{

    public GameObject floor;

    int numberOfWallTypes = 5;

    private GameObject[] wall_types = new GameObject[5]; // 5
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
            string myString = i.ToString();
            wall_types[i] = Resources.Load<GameObject>("wall_types/prefabs/asylum_walls"+ myString);
            Debug.Log(wall_types.Length);
        }
        float width = floor.transform.localScale.x;
        float height = floor.transform.localScale.z;
        Debug.Log(wall_types.Length);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("SpawnWalls", 3);
    }

    void SpawnWalls()
    {
        while (count < 50)
        {
            int randInt = random.Next(numberOfWallTypes);
            //Debug.Log(WallType.Window);

            Quaternion rot = Quaternion.Euler(0, storeRotY, 0);

            GameObject newWall = Instantiate(wall_types[randInt], storeSpawnPoint, rot);
            Vector3 newSpawnPoint = new Vector3();
            foreach (Transform child in newWall.transform)
            {
                if (child.name == "SpawnPoint")
                {
                    newSpawnPoint = child.position;
                }
            }

            //Vector3 newSpawnPoint = newWall.GetComponent<WallInfo>().spawnPoint;

            storeSpawnPoint.x = newSpawnPoint.x;
            storeSpawnPoint.z = newSpawnPoint.z;
            if (newWall.GetComponent<WallInfo>().typeOfWall == WallInfo.WallType.Concave)
                storeRotY += 90;
            //if (randInt == 2)
            //    storeRotY += 90;
            //if (randInt == 3)
            //    storeRotY += 180;
            count++;
        }
    }
}
