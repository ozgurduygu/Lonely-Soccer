using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [SerializeField] private LineRenderer trajectoryLine;

    public Vector3[] CalculateTrajectory(Vector3 initialVelocity)
    {
        var velocity = initialVelocity;
        var positions = new List<Vector3>();
        var collidersLayerMask = LayerMask.GetMask("Colliders");

        RaycastHit hit;

        if(velocity.magnitude > 0)
        {
            positions.Add(transform.position);
            
            Ray ray = new Ray(transform.position, velocity);
            //if(Physics.Raycast(transform.position, velocity, out hit, velocity.magnitude, collidersLayerMask))
            if(Physics.SphereCast(ray, transform.localScale.x, out hit, velocity.magnitude, collidersLayerMask))
            {
                Debug.Log(hit.normal);

                var reflection = Vector3.Reflect(velocity, hit.normal);

                positions.Add(hit.point);

                positions.Add(hit.point + reflection);

                Debug.DrawLine(transform.position, hit.point, Color.red); // Draw origin to hit Vector
                Debug.DrawLine(hit.point, hit.normal * reflection.magnitude, Color.blue); // Draw Normal Vector
                Debug.DrawLine(hit.point, reflection, Color.magenta); // Draw Reflection Vector
            }
            else
            {
                var lastPosition = positions[positions.Count-1] + velocity;
                Debug.DrawLine(transform.position, lastPosition, Color.red);
                positions.Add(lastPosition);
            }
            
            trajectoryLine.positionCount = positions.Count;
            trajectoryLine.SetPositions(positions.ToArray());
            positions.Clear();
        }
        return new Vector3[]{};
    }

    public IEnumerator MoveThroughPointsCoroutine(Vector3[] points)
    {
        yield return null;
    }
}
