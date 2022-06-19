using UnityEngine;

public class TouchController : MonoBehaviour
{
    public static bool active = true;

    public static float touchTolerance = 1f;

    public delegate void TouchEvent(Vector3 position);
    public static event TouchEvent OnTouchBegin;
    public static event TouchEvent OnTouchDrag;
    public static event TouchEvent OnTouchComplete;

    public static Touchable activeTouchable;

    private bool _isDragging;

    private void Update()
    {
        if (!active)
            return;

        var position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;

            if (OnTouchBegin != null)
            {
                UpdateActiveTouchable(position);
                OnTouchBegin(position);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;

            if (OnTouchComplete != null)
            {
                ClearActiveTouchable();
                OnTouchComplete(position);
            }
        }
        else if (_isDragging)
        {
            if (OnTouchDrag != null)
            {
                UpdateActiveTouchable(position);
                OnTouchDrag(position);
            }
        }
    }

    private void ClearActiveTouchable()
    {
        if (activeTouchable != null)
        {
            activeTouchable.isTouched = false;
        }

        activeTouchable = null;
    }

    private void UpdateActiveTouchable(Vector3 position)
    {
        var hit = HitCheck(position);

        var touchable = hit.collider.gameObject.GetComponent<Touchable>();

        ClearActiveTouchable();

        if (touchable != null)
        {
            touchable.isTouched = true;
            activeTouchable = touchable;
        }
    }

    private static RaycastHit HitCheck(Vector3 position)
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(position);

        Physics.SphereCast(ray, touchTolerance, out hit);

        return hit;
    }

    public static float InputFactoredByAngle(float angle)
    {
        var radian = angle * Mathf.Deg2Rad;

        var inputX = Input.GetAxis("Mouse X") * Mathf.Cos(radian);
        var inputY = Input.GetAxis("Mouse Y") * Mathf.Sin(radian) * -1;

        return inputX + inputY;
    }
}
