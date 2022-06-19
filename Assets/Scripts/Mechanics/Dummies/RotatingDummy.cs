using System.Collections.Generic;
using UnityEngine;

public class RotatingDummy : Dummy
{
    [SerializeField] private int directionSelector = 0;
    [SerializeField] private List<float> directions = new List<float> { 0f, 45f, 90f };

    private bool _shouldRotate;
    private bool _hasTouched;

    private Vector3 _localNormal = Vector3.forward;

    public override Vector3 Bounce(Vector3 entryVector)
    {
        var worldNormal = transform.rotation * _localNormal;
        return Vector3.Reflect(entryVector, worldNormal);
    }

    public override void Interact(Vector3 position)
    {
        directionSelector++;
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        var selector = SelectorInRange(directionSelector);
        var direction = directions[selector];
        SetDirection(direction);
    }

    private int SelectorInRange(int selector)
    {
        return Mathf.Abs(selector) % directions.Count;
    }

    private void SetDirection(float direction)
    {
        transform.rotation = Quaternion.Euler(0, direction, 0);
    }
    
    private void OnValidate()
    {
        UpdateDirection();
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
}
