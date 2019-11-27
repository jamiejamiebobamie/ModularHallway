using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInfo : MonoBehaviour
{

    static int numberOfWalls;

    public enum WallType // should i declare this here or in the ConstructHallways class?
    {
        Window,
        Bare,
        Concave,
        Convex,
        Door,
        Fork,
    }

    System.Random rand = new System.Random();

    int randPercentage = 15;

    List<Vector3> spawnPoints = new List<Vector3>();

    public WallType typeOfWall;

    // an empty game object placed at the position to spawn the next wall piece
    [SerializeField] private GameObject spawnPoint_gameObject;
    [SerializeField] private GameObject spawnPoint_gameObject2;

    public Vector3 spawnPoint;
    public Vector3 spawnPoint2;

    private Quaternion rot = Quaternion.identity;

    // the props that can be spawned by this wall type
    [SerializeField] protected GameObject[] props;

    public WallInfo()
    {
        numberOfWalls++;
        randPercentage = rand.Next(0, 15);

        //Debug.Log(numberOfWalls);
    }

    void Start()
    {
        spawnPoint = spawnPoint_gameObject.GetComponent<WallSpawnPoint>().spawnPoint;

        // forks have two spawnpoints.
        spawnPoint2 = spawnPoint_gameObject2.GetComponent<WallSpawnPoint>().spawnPoint;

        if (randPercentage < 3)
        //if (true)
        {
            int randInt = rand.Next(0, props.Length-1);
            Props prop = props[randInt].GetComponent<Props>();
            string nameOfProp = prop.name;
            //Debug.Log(name);

            prop.attachedWall = gameObject;
            switch (nameOfProp)
            {
                case "FireExtinguisher":
                    foreach (Transform child in transform)
                        if (child.name == "FireExtinguisher_spawnPoints")
                        {
                            foreach (Transform childrenOfChildren in child.transform)
                                spawnPoints.Add(childrenOfChildren.position);
                        }
                    break;
                case "FloorPlant":
                    foreach (Transform child in transform)
                        if (child.name == "FloorPlant_spawnPoints")
                        {
                            foreach (Transform childrenOfChildren in child.transform)
                                spawnPoints.Add(childrenOfChildren.position);
                        }
                    break;
                case "TrashCan":
                    foreach (Transform child in transform)
                        if (child.name == "TrashCan_spawnPoints")
                        {
                            foreach (Transform childrenOfChildren in child.transform)
                                spawnPoints.Add(childrenOfChildren.position);
                        }
                    break;
                case "Radiator":
                    foreach (Transform child in transform)
                        if (child.name == "Radiator_spawnPoints")
                        {
                            foreach (Transform childrenOfChildren in child.transform)
                                spawnPoints.Add(childrenOfChildren.position);
                        }
                    break;
                case "WetFloorSign":
                    foreach (Transform child in transform)
                        if (child.name == "WetFloorSign_spawnPoints")
                        {
                            foreach (Transform childrenOfChildren in child.transform)
                                spawnPoints.Add(childrenOfChildren.position);
                        }
                    break;
                case "WheelChair":
                    foreach (Transform child in transform)
                        if (child.name == "WheelChair_spawnPoints")
                        {
                            foreach (Transform childrenOfChildren in child.transform)
                                spawnPoints.Add(childrenOfChildren.position);
                        }
                    break;
                default:
                    Debug.Log("Default case");
                    break;
            }
            //Debug.Log(spawnPoints.Count);
            int anotherRandInt = rand.Next(0, spawnPoints.Count);
            Vector3 randomSpawnPoint = spawnPoints[anotherRandInt];
            //Quaternion randomRot = Quaternion.Euler(0, rand.Next(0, 361), 0);
            GameObject wallProp = Instantiate(props[randInt], randomSpawnPoint, Quaternion.identity);
        }
    }
       
}

/*
 * wall
 types:        spawnpoint:               rotation?:          prop_types:
   enum:           Vector3:               Quaternion:       GameObject[]:
        Window,     next spawnPoint,       no rotation,       trash can, floor plant, wet floor sign  
        Bare,       next spawnPoint,       no rotation,       trash can, fire exstinguisher, floor plant, radiator, wet floor sign
        Concave,    next spawnPoint,       rotation,          fire extinguisher, wheel chair, wet floor sign
        Convex,     next spawnPoint,       rotation,          fire extinguisher, wet floor sign
        Door,       next spawnPoint,       no rotation,       wet floor sign,
     */
