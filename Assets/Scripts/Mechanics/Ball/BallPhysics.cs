using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] public float gravity = -100;

    [SerializeField] private GameObject lineRenderer;
    private Trajectory trajectory;
    [SerializeField] private int maxIteration = 12;

    private Vector3 _spawnPosition;

    private void Awake()
    {
        _spawnPosition = transform.position;
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
                if(hit.collider.CompareTag("GoalPost"))
                {
                    Debug.Log("scores");                
                    trajectory.GetComponent<LineRenderer>().materials[0].SetVector("_EmissionColor", Color.green * 3);
                }
                else
                    trajectory.GetComponent<LineRenderer>().materials[0].SetVector("_EmissionColor", Color.white * 3);
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
        // Attach camera to follow the ball
        Camera.main.GetComponent<CameraControls>().SetCamera(CameraControls.Camera.ball);
        var points = trajectory.ProcessTrajectory();
        StartCoroutine(MoveBall(points));
    }

    private IEnumerator MoveBall(Vector3[] points)
    {
        yield return new WaitForSeconds(0.2f);

        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            var origin = transform.position;
            var destination = points[i];
            var distanceX = destination.x - origin.x;
            var distanceY = destination.y - origin.y;
            var distanceZ = destination.z - origin.z;

            var peakHeight = 8f;
            var horizontalDistance = new Vector3(distanceX, 0, distanceZ);

            var travelTime = Mathf.Sqrt(-2f * peakHeight / gravity) + Mathf.Sqrt(2f * (distanceY - peakHeight) / gravity);
            var verticalVelocity = Vector3.up * Mathf.Sqrt(-2f * gravity * peakHeight);
            var horizontalVelocity = horizontalDistance / travelTime;

            Physics.gravity = Vector3.up * gravity;
                if(i == points.Length - 1)
                    GetComponent<Rigidbody>().drag = 1f;

            GetComponent<Rigidbody>().velocity = verticalVelocity + horizontalVelocity;


            while (travelTime > 0f)
            {
                travelTime -= Time.deltaTime;
                yield return null;
            }
        }

        // Shoot finished, return camera and ball!
        yield return new WaitForSeconds(1);

        Camera.main.GetComponent<CameraControls>().SetCamera(CameraControls.Camera.top);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().drag = 0f;
        transform.position = _spawnPosition;
    }
}
