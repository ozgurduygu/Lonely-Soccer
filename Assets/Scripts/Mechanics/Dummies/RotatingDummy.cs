using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDummy : Dummy
{
    [SerializeField] private List<float> directions = new List<float>{0f, 45f, 90f};
    
    private Vector3 _localNormal = Vector3.forward;
    
    private int _directionSelector;
    
    public override Vector3 Bounce(Vector3 entryVector)
    {
        var worldNormal = transform.rotation * _localNormal;
        return Vector3.Reflect(entryVector, worldNormal);
    }

    public override void Interact()
    {
        _directionSelector++;
        var selector = _directionSelector % directions.Count;
        var direction = directions[selector];
        transform.rotation = Quaternion.Euler(0, direction, 0);
    }
}
