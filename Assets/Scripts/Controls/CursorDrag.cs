using UnityEngine;

public class CursorDrag : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float minDragMagnitude = .15f;

    private BallPhysics _ballPhysics;
    private CursorDisplayer _cursorDisplayer;

    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private Vector3 _ballSpawnPosition;

    private void Awake()
    {
        _cursorDisplayer = GetComponent<CursorDisplayer>();
        _ballPhysics = ball.GetComponent<BallPhysics>();

        _ballSpawnPosition = ball.transform.position;
    }

    public void DragStart(Vector3 position)
    {
        _startPoint = position;
        _cursorDisplayer.SetBackgroundPosition(_startPoint);
        _cursorDisplayer.CursorDragStart();
    }

    public void DragUpdate(Vector3 position)
    {
        _cursorDisplayer.SetCursorPosition(position);

        var initialVelocity = new Vector3(_cursorDisplayer.Value.x, 0, _cursorDisplayer.Value.y) * -40f;
        _ballPhysics.CalculateTrajectory(initialVelocity);
    }

    public void DragEnd(Vector3 position)
    {
        _cursorDisplayer.CursorDragEnd();

        if (_cursorDisplayer.Value.magnitude < minDragMagnitude)
        {

        }
    }
}
