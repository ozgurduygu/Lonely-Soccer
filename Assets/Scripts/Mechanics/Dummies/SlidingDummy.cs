using UnityEngine;

public class SlidingDummy : RotatingDummy
{
    [Range(-1f, 1f)]
    [SerializeField] private float slidePosition = 0f;

    [SerializeField] private float slideScalar = 2f;

    [SerializeField] private float inputSensitivity = 0.3f;

    private Vector3 _cachedPosition;

    private LineRenderer _lineRenderer;

    private bool _doInteract;

    public override void Interact(Vector3 position)
    {
        slidePosition -= Input.GetAxis("Mouse Y") * inputSensitivity;
        ApplySlidePosition();
    }

    public new void OnTouchBegin(Vector3 position, bool isTouched)
    {
        _doInteract = isTouched;
    }

    public new void OnTouchDrag(Vector3 position, bool isTouched)
    {
        if (_doInteract)
        {
            Interact(position);
        }
    }

    public new void OnTouchComplete(Vector3 position, bool isTouched)
    {
        _doInteract = false;
    }

    private void OnValidate()
    {
        ApplySlidePosition();
        DrawTrack();
    }

    private void ApplySlidePosition()
    {
        slidePosition = Mathf.Clamp(slidePosition, -1f, 1f);

        var slideAmount = transform.right * slidePosition * slideScalar;

        if (transform.hasChanged)
            _cachedPosition = transform.position - slideAmount;

        transform.position = _cachedPosition + slideAmount;
        transform.hasChanged = false;
    }

    private void DrawTrack()
    {
        var halfWidth = transform.localScale.x / 2;
        var offsetX = transform.right * (slideScalar + halfWidth);
        var offsetY = Vector3.down * halfWidth + Vector3.up * 0.1f;

        var minPoint = _cachedPosition - offsetX + offsetY;
        var maxPoint = _cachedPosition + offsetX + offsetY;

        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();

        var points = new Vector3[] { minPoint, maxPoint };
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(points);
    }

}
