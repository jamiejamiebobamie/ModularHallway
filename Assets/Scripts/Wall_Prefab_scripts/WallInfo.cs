using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInfo : MonoBehaviour
{
    public enum WallType // should i declare this here or in the ConstructHallways class?
    {
        Window,
        Bare,
        Concave,
        Convex,
        Door,
    }

    List<Vector3> spawnPoints = new List<Vector3>();
    System.Random rand = new System.Random();

    protected WallType typeOfWall;

    // an empty game object placed at the position to spawn the next wall piece
    [SerializeField] private GameObject spawnPoint_gameObject;
    public Vector3 spawnPoint;

    private Quaternion rot = Quaternion.identity;

    // the props that can be spawned by this wall type
    [SerializeField] protected GameObject[] props;

    void Start()
    {
        spawnPoint = spawnPoint_gameObject.GetComponent<WallSpawnPoint>().spawnPoint;

        int randThird = rand.Next(0, 8);
        if (randThird < 3)
        {
            int randInt = rand.Next(0, props.Length);
            Props prop = props[randInt].GetComponent<Props>();
            string name = prop.name;
            //Debug.Log(name);

            prop.attachedWall = gameObject;
            switch (name)
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
