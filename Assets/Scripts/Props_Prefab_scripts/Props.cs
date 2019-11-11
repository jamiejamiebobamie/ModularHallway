using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [Range(1, 10)]
    protected int speed;

    public enum PropType // should i declare this here or in the ConstructHallways class?
    {
        FireExtinguisher,
        TrashCan,
        FloorPlant,
        Radiator,
        WetFloorSign,
        WheelChair,
    }

    protected PropType typeOfProp;

    public Vector3[] spawnPoints;
    private Vector3 spawnLocation; // need this to be a single vector pulled from a range of values base on the prop's "area"

    public GameObject attachedWall;

    private void Start()
    {
        //while (doOnce && attachedWall != null)
        //{
        //    float minIntoHallway = attachedWall.transform.position.x - attachedWall.transform.localScale.x / 5f;
        //    float maxIntoHallway = attachedWall.transform.position.x + attachedWall.transform.localScale.x / 1.5f;
        //    float minAlongWall = attachedWall.transform.position.z - attachedWall.transform.localScale.z / 2f;
        //    float maxAlongWall = attachedWall.transform.position.z + attachedWall.transform.localScale.z / 2f;

        //    //attachedWall.transform.right*2;
        //    Vector3 x = attachedWall.transform.right * .5f;
        //    Vector3 z = attachedWall.transform.forward * .1f;
        //    //Debug.Log(x);
        //    //float x = Random.Range(minIntoHallway, maxIntoHallway);
        //    //float z = Random.Range(minAlongWall, maxAlongWall);

        //    // must spawn on wall
        //    //spawnLocation = x.x;
        //    spawnLocation = new Vector3(attachedWall.transform.position.x+x.x, attachedWall.transform.position.y + 1.285f, attachedWall.transform.position.z);//*+z.z*12f);

        //    Debug.Log(attachedWall.transform.right*3f);

        //    transform.position = spawnLocation;
        //    doOnce = false;
        //}
    }

    private void Update()
    {

        if (transform.position.y < -1f)
        {
            Destroy(gameObject,2f);
        }
        //if (spawnLocation != transform.position)
        //{
        //    spawnLocation = new Vector3(attachedWall.transform.position.x + 1f, attachedWall.transform.position.y + 1.285f, attachedWall.transform.forward.z * 2.5f);

        //    Debug.Log(attachedWall.transform.right * 3f);

        //    transform.position = spawnLocation;
        //}

    }
}





//props:                      speed(1-10, 10=fastest)       area               quirk
//        fire extinguisher           5                      on_wall            1/3 chance of being interacted with by a guard("why is this on the floor?")
//        trash can                   8                      wall_floor         spawns trash/debris that guards will follow to your location(banana peel, crumpled paper, etc.hard to follow if the guard cannot see the next piece of trash in the array, will create a vector from the second to last piece of garbage that the guard can see to the last piece of garbage the guard can see and head in that direction)
//        floor plant                 10                     wall_floor         leaves a dusting of soil in its wake that is even denser and easier to follow than the trash can.impossible to lose trail.
//        radiator                    7                      wall_floor         player must lose 1 of three of its health points to turn into the radiator (burns player by touching.)
//        wheel chair                 3                      anywhere           only spawned in concave corners.
//        wet floor sign              7                      anywhere           attracts all nearby guards when player touches object to copy it.