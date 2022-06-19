using UnityEngine;

public class AutoDummy : Dummy
{
    public GameObject target;
    
    public override Vector3 Bounce(Vector3 entryVector)
    {
        if(target == null) return Vector3.zero;
        
        var distance = target.transform.position - transform.position;
        var direction = distance.normalized;
        direction.y = 0;
        
        var reflection = direction * entryVector.magnitude;
        return reflection;
    }

    public override void Interact(Vector3 position) {}
}
