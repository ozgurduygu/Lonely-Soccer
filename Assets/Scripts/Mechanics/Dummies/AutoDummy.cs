using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDummy : Dummy
{
    public GameObject target;
    
    public override Vector3 Shoot(Vector3 entryVector)
    {
        var direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        return direction * entryVector.magnitude;
    }
}
