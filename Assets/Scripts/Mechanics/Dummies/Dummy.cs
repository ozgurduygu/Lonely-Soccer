using UnityEngine;

public abstract class Dummy : MonoBehaviour
{
    public abstract Vector3 Bounce(Vector3 entryVelocity);

    public abstract void Interact(Vector3 position);
}
