using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public Quaternion rotation;
    public Vector3 position;

    public SpawnPoint()
    {
        rotation = Quaternion.identity;
        position = Vector3.zero;

    }

    private void Start()
    {
        rotation = transform.rotation;
        position = transform.position;
    }
}
