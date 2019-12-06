using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_rotate : MonoBehaviour
{
    // going to have it only follow the player as opposed to other patients or
        // guards... it's creepier if the player is the sole focus.

    [SerializeField] GameObject focusOfCamera;



    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("FollowPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        WatchPlayer();
    }


    private IEnumerator FollowPlayer()
    {
        Vector3 lookAtDirection =
            Vector3.Normalize(focusOfCamera.transform.position
                - transform.position);

        Quaternion rotationTowardPlayer =
            Quaternion.LookRotation(lookAtDirection, transform.up);

        transform.rotation =
            Quaternion.Slerp(transform.localRotation,
            rotationTowardPlayer, 0.05f);

        yield return null;
    }


    private void WatchPlayer()
    {
        Vector3 lookAtDirection =
            Vector3.Normalize(focusOfCamera.transform.position
                - transform.position);

        Quaternion rotationTowardPlayer =
            Quaternion.LookRotation(lookAtDirection, transform.up);

        transform.rotation = Quaternion.Slerp(transform.rotation,
            rotationTowardPlayer, 0.05f);
    }
}
