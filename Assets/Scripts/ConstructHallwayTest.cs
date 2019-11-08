using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructHallwayTest : MonoBehaviour
{
    [SerializeField] GameObject[] pieces;

    System.Random random = new System.Random();

    Stack<SpawnPoint> spawnPoints = new Stack<SpawnPoint>();

    SpawnPoint firstSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {

        foreach (Transform child in transform)
        {
            firstSpawnPoint = child.GetComponent<SpawnPoint>();
        }

        //firstSpawnPoint.rotation = Quaternion.identity;
        //firstSpawnPoint.position = Vector3.zero;

        if (firstSpawnPoint != null)
            spawnPoints.Push(firstSpawnPoint);

        int count = 0;
        int index = 0;

        while (count < 50)
        {
            index = random.Next(0,2); // not inclusive max value
            SpawnPoint nextSpawnPoint = spawnPoints.Pop();
            GameObject nextPiece = Instantiate(pieces[index], nextSpawnPoint.position, nextSpawnPoint.rotation);

            foreach(Transform children in nextPiece.transform)
            {
                SpawnPoint test = children.GetComponent<SpawnPoint>();
                Debug.Log(children.name);
                if (test.rotation != Quaternion.identity) // this isn't what
                    // I wanted to do, but a ' is not null'
                    // test was not working...
                {
                    spawnPoints.Push(test);
                }
            }
            count++;
        }

        foreach (SpawnPoint sp in spawnPoints)
        {
            Debug.Log(sp.position);
        }
    }
}
