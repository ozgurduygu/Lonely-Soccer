using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float minDragMagnitude = .15f;

    [SerializeField] private BallPhysics ballPhysics;
    [SerializeField] private CursorDisplayer cursorDisplayer;

    private bool _trajectoryActive;

    private bool _isActive;

    public void DragStart(Vector3 position, bool isTouched)
    {
        if (TouchController.activeTouchable == null)
        {
            _isActive = true;
        }

        if (_isActive)
        {
            cursorDisplayer.SetBackgroundPosition(position);
            cursorDisplayer.CursorDragStart();
        }
    }

    public void DragUpdate(Vector3 position, bool isTouched)
    {
        if (_isActive)
        {
            cursorDisplayer.SetCursorPosition(position);

            if (cursorDisplayer.Value.magnitude >= minDragMagnitude)
            {
                if (!_trajectoryActive)
                {
                    GameObject.Find("Ball Trajectory").GetComponent<Trajectory>().FadeIn();
                    _trajectoryActive = true;
                }
                var initialVelocity = new Vector3(cursorDisplayer.Value.x, 0, cursorDisplayer.Value.y) * -40f;
                ballPhysics.CalculateTrajectory(initialVelocity);
            }
            else
            {
                if (_trajectoryActive)
                {
                    GameObject.Find("Ball Trajectory").GetComponent<Trajectory>().FadeOut();
                    _trajectoryActive = false;
                }
                ballPhysics.EndTrajectory();
            }
        }
    }

    public void DragEnd(Vector3 position, bool isTouched)
    {
        if (_isActive)
        {
            _isActive = false;
            cursorDisplayer.CursorDragEnd();
            if (cursorDisplayer.Value.magnitude >= minDragMagnitude)
            {
                ballPhysics.Shoot();
            }

            if (_trajectoryActive)
            {
                GameObject.Find("Ball Trajectory").GetComponent<Trajectory>().FadeOut();
                _trajectoryActive = false;
            }
        }
    }
}
