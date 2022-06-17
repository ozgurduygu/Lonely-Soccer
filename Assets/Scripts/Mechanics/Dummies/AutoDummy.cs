using UnityEngine;

public class AutoDummy : Dummy
{
    public GameObject target;
    
    public override Vector3 Bounce(Vector3 entryVector)
    {
        if(target == null)
            return Vector3.zero;
        
        var direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        return direction * entryVector.magnitude;
    }

    public override void Interact(Vector3 position)
    {
        return;
    }
}
