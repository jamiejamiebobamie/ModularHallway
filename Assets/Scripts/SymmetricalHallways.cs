using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymmetricalHallways : MonoBehaviour
{

    GameObject floor, begin, exit;

    float randX;
    float randZ;

    System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        // generate a floor
        floor = GameObject.CreatePrimitive(PrimitiveType.Plane) as GameObject;
        floor.transform.localScale = new Vector3 (100,100,100);

        //generate x coordinates for start and exit areas
        double randomDouble = random.NextDouble();
        double randVal = randomDouble * (double)floor.transform.localScale.x*4
            - (double)floor.transform.localScale.x*4;
        randX = (float)randVal;

        //generate z coordinates for start and exit areas
        randomDouble = random.NextDouble();
        randVal = randomDouble * (double)floor.transform.localScale.z*5
            - (double)floor.transform.localScale.z * 5;
        randZ = (float)randVal;

        begin = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
        begin.transform.position = new Vector3(randX+100, 0, randZ);
        begin.name = "start";

        exit = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
        exit.transform.position = new Vector3(-randX-100, 0, randZ);
        exit.name = "exit";



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
