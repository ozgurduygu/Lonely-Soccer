using System.Collections;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] public float gravity = -100;

    [SerializeField] private GameObject lineRenderer;
    
    [SerializeField] private Trajectory trajectory;
    
    [SerializeField] private int maxTrajectoryBounce = 12;

    public Vector3[] CalculateTrajectory(Vector3 velocity)
    {
        trajectory.ClearPoints();

        var reflection = velocity;
        var collidersLayerMask = LayerMask.GetMask("Colliders");

        trajectory.AddPoint(transform.position);


        Ray ray = new Ray(transform.position, reflection);

        var i = 0;
        var doCheckHit = true;
        while (doCheckHit && i < maxTrajectoryBounce)
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

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("GoalPost"))
        {
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void EndTrajectory()
    {
    }

    public void Shoot()
    {
        // Attach camera to follow the ball
        TouchController.active = false;

        Camera.main.GetComponent<CameraControls>().SetCamera(CameraControls.Camera.ball);
        var points = trajectory.ProcessTrajectory();
        StartCoroutine(MoveBall(points));
    }

    private IEnumerator MoveBall(Vector3[] points)
    {
        var cachedPosition = transform.position;

        yield return new WaitForSeconds(0.2f);

        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            var origin = transform.position;
            var destination = points[i];
            destination.y += 2f;
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

        transform.position = cachedPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        TouchController.active = true;
    }
}
