using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Ball _ball;

    [SerializeField] private float gravity = -100;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private int maxTrajectoryBounce = 12;

    private LayerMask _dummiesLayerMask;

    private void Awake()
    {
        _dummiesLayerMask = LayerMask.GetMask("Dummies");
    }

    private void OnValidate()
    {
        UpdateGravity(gravity);
    }

    public List<Vector3> CalculateTrajectory(Vector3 motion)
    {
        var trajectoryPoints = new List<Vector3>();

        var trajectoryStartingPosition = transform.position;
        trajectoryPoints.Add(trajectoryStartingPosition);

        var bounceOffPoints = CalculateBounceOffPoints(ref motion);
        trajectoryPoints.AddRange(bounceOffPoints);

        var ballStopPosition = trajectoryPoints.Last() + motion;
        trajectoryPoints.Add(ballStopPosition);

        return trajectoryPoints;
    }

    private List<Vector3> CalculateBounceOffPoints(ref Vector3 reflection)
    {
        var bounceOffPoints = new List<Vector3>();

        var limit = maxTrajectoryBounce;
        var shouldCheckBounce = limit > 0;

        RaycastHit hit;
        var ray = new Ray(transform.position, reflection);

        while (shouldCheckBounce)
        {
            var hits = Physics.SphereCast(ray, transform.localScale.x, out hit, reflection.magnitude, _dummiesLayerMask);

            if (hits)
            {
                bounceOffPoints.Add(hit.point);

                reflection = BounceOffDummy(hit, reflection);
                ray.origin = hit.point;
                ray.direction = reflection;
            }

            limit--;
            shouldCheckBounce = hits && limit > 0;
        }

        return bounceOffPoints;
    }

    private Vector3 BounceOffDummy(RaycastHit hit, Vector3 reflection)
    {
        var dummy = hit.collider.gameObject.GetComponent<Dummy>();

        if (dummy != null)
        {
            return dummy.Bounce(reflection);
        }
        else
        {
            return reflection;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        CheckGoalPostHit(collider);
    }

    private void CheckGoalPostHit(Collider collider)
    {
        if (collider.CompareTag("GoalPost"))
        {
            _ball.Score();
        }
    }

    public IEnumerator MoveBallCoroutine(Vector3[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i == points.Length - 1)
                _rigidbody.drag = 1f;

            yield return StartCoroutine(MoveBallToPoint(points[i]));
        }
    }

    private IEnumerator MoveBallToPoint(Vector3 destination)
    {
        var origin = transform.position;
        var distanceX = destination.x - origin.x;
        var distanceY = destination.y - origin.y;
        var distanceZ = destination.z - origin.z;

        var minPeakHeight = 1f;
        var maxPeakHeight = 8f;
        var horizontalDistance = new Vector3(distanceX, 0, distanceZ);
        var peakHeight = Mathf.Lerp(minPeakHeight, maxPeakHeight, horizontalDistance.magnitude / 100);

        var travelTime = Mathf.Sqrt(-2f * peakHeight / gravity) + Mathf.Sqrt(2f * (distanceY - peakHeight) / gravity);
        var verticalVelocity = Vector3.up * Mathf.Sqrt(-2f * gravity * peakHeight);
        var horizontalVelocity = horizontalDistance / travelTime;

        _rigidbody.velocity = verticalVelocity + horizontalVelocity;

        var waitForBallToMove = new WaitForSeconds(travelTime);
        yield return waitForBallToMove;
    }

    public void UpdateGravity(float gravity)
    {
        Physics.gravity = Vector3.up * gravity;
    }

    public void ResetPhysics()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.drag = 0f;
    }
}
