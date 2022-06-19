using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float minDragMagnitude = .15f;
    [SerializeField] private CursorDisplayer cursorDisplayer;
    [SerializeField] private Ball ball;    

    private bool _isActive;

    public void OnTouchBegin(Vector3 position, bool isTouched)
    {
        // Ignore touch if any other touchable has been touched.
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

            // Aim the ball to the opposite direction of drag value. 
            var strength = new Vector3(cursorDisplayer.Value.x, 0, cursorDisplayer.Value.y) * -1f;
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

        _isActive = false;
        ball.trajectory.FadeOut();
        cursorDisplayer.CursorDragEnd();
    }
}
