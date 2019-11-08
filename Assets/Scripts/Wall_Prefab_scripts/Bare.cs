using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bare : WallInfo
{
    System.Random rand = new System.Random();

    // having issues with all or no items being spawned
    // i think it's sharing the same random value across all instances of WallInfo...
    // want to keep code dry so it'd be best to create the bool per child and pass the bool
    // to the parent's constructor method... (idk how... using base?)

    public Bare()
    {        
        typeOfWall = WallType.Bare;
    }


}
