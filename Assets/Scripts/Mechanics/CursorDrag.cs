using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDrag : MonoBehaviour
{
    private CursorDisplayer _cursorDisplayer;

    private bool _isDragging;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private void Start()
    {
        _cursorDisplayer = GetComponent<CursorDisplayer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _startPoint = Input.mousePosition;
            _cursorDisplayer.SetBackgroundPosition(_startPoint);
            _cursorDisplayer.CursorDragStart();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            _endPoint = Input.mousePosition;
            _cursorDisplayer.CursorDragEnd();
            Debug.Log("Vector:" + _cursorDisplayer.GetDragValue());
            Debug.Log("Magnitude:" + _cursorDisplayer.GetDragValue().magnitude);
        }

        if (_isDragging)
        {
            _cursorDisplayer.SetCursorPosition(Input.mousePosition);
        }
    }
}
