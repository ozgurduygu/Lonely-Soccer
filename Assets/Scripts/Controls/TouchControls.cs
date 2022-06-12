using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    [SerializeField] private float touchTolerance = 1f;

    private bool _isDragging;

    private CursorDrag cursorDrag;
    private Dummy _dummy;

    private void Awake()
    {
        cursorDrag = GetComponentInChildren<CursorDrag>();
    }

    private void Update()
    {
        var position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            OnTouchBegin(position);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnTouchComplete(position);
        }
        else if (_isDragging)
        {
            OnTouchDrag(position);
        }
    }

    private void OnTouchBegin(Vector3 position)
    {
        _isDragging = true;

        _dummy = DummyTouchCheck(position);
        
        if(_dummy == null)
        {
            cursorDrag.DragStart(Input.mousePosition);
        }
    }

    private void OnTouchDrag(Vector3 position)
    {
        if (_dummy == null)
            cursorDrag.DragUpdate(Input.mousePosition);
        else
        {
            var slidingDummy = _dummy.GetComponent<SlidingDummy>();

            if(slidingDummy != null)
                slidingDummy.Slide(position);
        }
    }

    private void OnTouchComplete(Vector3 position)
    {
        _isDragging = false;

        if(_dummy != null)
        {
            _dummy.Interact();
        }
        else
        {
            cursorDrag.DragEnd(Input.mousePosition);
        }
    }

    private Dummy DummyTouchCheck(Vector3 position)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(position);
        
        Physics.SphereCast(ray, touchTolerance, out hit);

        return hit.transform.GetComponent<Dummy>();
    }
}
