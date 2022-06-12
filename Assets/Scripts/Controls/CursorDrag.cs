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

    private bool _trajectoryActive;

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

        if(_cursorDisplayer.Value.magnitude >= minDragMagnitude)
        {
            if(!_trajectoryActive)
            {
                GameObject.Find("BallTrajectory").GetComponent<Trajectory>().FadeIn();
                _trajectoryActive = true;
            }
            var initialVelocity = new Vector3(_cursorDisplayer.Value.x, 0, _cursorDisplayer.Value.y) * -40f;
            _ballPhysics.CalculateTrajectory(initialVelocity);
        }
        else
        {
            if(_trajectoryActive)
            {
                GameObject.Find("BallTrajectory").GetComponent<Trajectory>().FadeOut();
                _trajectoryActive = false;
            }
            _ballPhysics.EndTrajectory();
        }
    }

    public void DragEnd(Vector3 position)
    {
        _cursorDisplayer.CursorDragEnd();
        GameObject.Find("BallTrajectory").GetComponent<Trajectory>().FadeOut();
        if (_cursorDisplayer.Value.magnitude >= minDragMagnitude)
        {
            _ballPhysics.Shoot();
        }
        
        if(_trajectoryActive)
        {
            GameObject.Find("BallTrajectory").GetComponent<Trajectory>().FadeOut();
            _trajectoryActive = false;
        }
    }
}
