using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float minDragMagnitude = .15f;

    [SerializeField] private CursorDisplayer cursorDisplayer;

    [SerializeField] private Ball ball;    

    private bool _isActive;

    public void OnTouchBegin(Vector3 position, bool isTouched)
    {
        if (TouchController.activeTouchable != null) return;

        _isActive = true;

        cursorDisplayer.SetBackgroundPosition(position);
        cursorDisplayer.CursorDragStart();
    }

    public void OnTouchDrag(Vector3 position, bool isTouched)
    {
        if (!_isActive) return;

        cursorDisplayer.SetCursorPosition(position);

        if (cursorDisplayer.Value.magnitude >= minDragMagnitude)
        {
            ball.trajectory.FadeIn();

            var strength = new Vector3(cursorDisplayer.Value.x, 0, cursorDisplayer.Value.y) * -40f;

            ball.Aim(strength);
        }
        else
        {
            ball.trajectory.FadeOut();
        }
    }

    public void OnTouchComplete(Vector3 position, bool isTouched)
    {
        if (!_isActive) return;

        if (cursorDisplayer.Value.magnitude >= minDragMagnitude)
        {
            ball.Shoot();
        }

        ball.trajectory.FadeOut();

        _isActive = false;

        cursorDisplayer.CursorDragEnd();
    }
}
