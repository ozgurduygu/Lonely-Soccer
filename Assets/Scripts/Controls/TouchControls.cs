using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
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
            _isDragging = true;
            OnTouchDown(position);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            OnTouchEnd(position);
        }
        else if (_isDragging)
        {
            OnTouchDrag(position);
        }
    }

    private void OnTouchDown(Vector3 position)
    {
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

    private void OnTouchEnd(Vector3 position)
    {

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
        
        Physics.Raycast(ray, out hit);

        return hit.transform.GetComponent<Dummy>();
    }
}
