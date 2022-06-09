using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectDummy : Dummy
{

    public override Vector3 Shoot(Vector3 hitNormal)
    {
        return Vector3.zero;
    }
}
