using System.Collections.Generic;
using UnityEngine;

public class RotatingDummy : Dummy
{
    [SerializeField] private List<float> directions = new List<float> { 0f, 45f, 90f };
    private int _directionSelector = 1;

    private Vector3 _localNormal = Vector3.forward;

    private bool _shouldRotate;
    private bool _hasTouched;

    public override Vector3 Bounce(Vector3 entryVector)
    {
        var worldNormal = transform.rotation * _localNormal;
        return Vector3.Reflect(entryVector, worldNormal);
    }

    public override void Interact(Vector3 position)
    {
        var selector = _directionSelector % directions.Count;
        var direction = directions[selector];

        transform.rotation = Quaternion.Euler(0, direction, 0);

        _directionSelector++;
    }

    public void OnTouchBegin(Vector3 position, bool isTouched)
    {
        _hasTouched = isTouched;
    }

    public void OnTouchDrag(Vector3 position, bool isTouched)
    {
        _shouldRotate = _hasTouched && isTouched;
    }

    public void OnTouchComplete(Vector3 position, bool isTouched)
    {
        if (_shouldRotate)
        {
            Interact(position);
        }

        _hasTouched = false;
        _shouldRotate = false;
    }

    private void OnValidate()
    {
        transform.rotation = Quaternion.Euler(0, directions[0], 0);
    }
}
