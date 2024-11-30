using System;
using UnityEngine;

public class GravityController
{
    static public Vector3 gravity = new Vector3(0, -9.81f, 0);

    // public static void ChangeGravityToX()
    // {
    //     gravity = new Vector3(-9.81f, 0, 0);
    // }
    
    public static void ChangeGravityToY()
    {
        gravity = new Vector3(0, -9.81f, 0);
    }
    
    public static void ChangeGravityToZ()
    {
        gravity = new Vector3(0, 0, 9.81f);
    }
}
