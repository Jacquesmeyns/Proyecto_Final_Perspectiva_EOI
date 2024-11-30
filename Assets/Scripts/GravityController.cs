using System;
using UnityEngine;

public class GravityController
{
    public static float gravityValue = 9.8f;
    static Vector3 gravity = new Vector3(0, -9.81f, 0);
    public static Vector3 Gravity => gravity; 
    
    // public static void ChangeGravityToX()
    // {
    //     gravity = new Vector3(-9.81f, 0, 0);
    // }
    //
    // public static void ChangeGravityToNegX()
    // {
    //     gravity = new Vector3(9.81f, 0, 0);
    // }
    //
    // public static void ChangeGravityToY()
    // {
    //     gravity = new Vector3(0, -9.81f, 0);
    // }
    //
    // public static void ChangeGravityToNegY()
    // {
    //     gravity = new Vector3(0, 9.81f, 0);
    // }
    //
    // public static void ChangeGravityToZ()
    // {
    //     gravity = new Vector3(0, 0, 9.81f);
    // }
    //
    // public static void ChangeGravityToNegZ()
    // {
    //     gravity = new Vector3(0, 0, -9.81f);
    // }

    public static bool ChangeGravityDirection(Vector3 newGravityDirection)
    {
        if (newGravityDirection * gravityValue == gravity)  //No se cambia la gravedad porque está igual
            return false;
        gravity = newGravityDirection * gravityValue;
        return true;
    }
}
