using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawnPoint : MonoBehaviour
{
    public Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;
    }
}
