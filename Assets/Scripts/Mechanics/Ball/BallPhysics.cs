using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [SerializeField] private Trajectory trajectory;
    [SerializeField] private int maxIteration = 12;
    
    private void Awake() {
        
    }

    public Vector3[] CalculateTrajectory(Vector3 velocity)
    {
        var reflection = velocity;
        var collidersLayerMask = LayerMask.GetMask("Colliders");

        trajectory.AddPoint(transform.position);


        Ray ray = new Ray(transform.position, reflection);

        var i = 0;
        var doCheckHit = true;
        while (doCheckHit && i < maxIteration)
        {
            RaycastHit hit;

            doCheckHit = Physics.SphereCast(ray, transform.localScale.x, out hit, reflection.magnitude, collidersLayerMask);

            if (doCheckHit)
            {
                var dummy = hit.collider.gameObject.GetComponent<Dummy>();


                if (dummy)
                    reflection = dummy.Shoot(reflection);

                Debug.Log(reflection);

                ray = new Ray(hit.point, reflection);

                trajectory.AddPoint(hit.point);
            }

            i++;
        }

        var lastPosition = trajectory.lastPoint + reflection;
        trajectory.AddPoint(lastPosition);

        trajectory.Draw();
        trajectory.Clear();

        return new Vector3[]{};
    }

    public IEnumerator MoveThroughPointsCoroutine(Vector3[] points)
    {
        yield return null;
    }
}
