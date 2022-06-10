using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDummy : Dummy
{
    public Vector3 localNormal = Vector3.forward;
    
    public override Vector3 Shoot(Vector3 entryVector)
    {
        var worldNormal = transform.rotation * localNormal;
        return Vector3.Reflect(entryVector, worldNormal);
    }
}
