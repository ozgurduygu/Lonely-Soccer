using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [SerializeField] private GameObject lineRenderer;
    private Trajectory trajectory;
    [SerializeField] private int maxIteration = 12;

    private void Awake()
    {
        trajectory = lineRenderer.GetComponent<Trajectory>();
    }

    public Vector3[] CalculateTrajectory(Vector3 velocity)
    {
        trajectory.ClearPoints();

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
                    reflection = dummy.Bounce(reflection);

                ray = new Ray(hit.point, reflection);

                trajectory.AddPoint(hit.point);
            }

            i++;
        }

        var lastPosition = trajectory.lastPoint + reflection;
        trajectory.AddPoint(lastPosition);

        trajectory.Draw();

        return new Vector3[] { };
    }

    public void EndTrajectory()
    {
    }

    public void Shoot()
    {
        var points = trajectory.ProcessTrajectory().ToArray();
        StartCoroutine(MoveBall(points));
    }

    private IEnumerator MoveBall(Vector3[] points)
    {
        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            var destination = points[i];
            var delta = 0f;
            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                delta += Time.deltaTime * 0.1f;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, 0.1f, speed);
                transform.position = Vector3.Lerp(transform.position, destination, delta);
                yield return null;
            }
        }
    }
}
