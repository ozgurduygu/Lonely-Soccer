using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDrag : MonoBehaviour
{
    [SerializeField]
    private GameObject _ball;
    [SerializeField]
    private float _minDragMagnitude = .15f;

    private CursorDisplayer _cursorDisplayer;

    private bool _isDragging;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private void Awake()
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
            
            Vector2 dragVector = _cursorDisplayer.GetDragVector;

            if(dragVector.magnitude > _minDragMagnitude)
            {
                Debug.Log("Vector: " + dragVector);
                Debug.Log("Magnitude:" + dragVector.magnitude);

                Rigidbody ballRigidbody = _ball.GetComponent<Rigidbody>();
                ballRigidbody.velocity = Vector3.zero;
                ballRigidbody.AddForce(new Vector3(dragVector.x, 1, dragVector.y) * -10, ForceMode.Impulse);
            }
            _cursorDisplayer.CursorDragEnd();
        }

        if (_isDragging)
        {
            _cursorDisplayer.SetCursorPosition(Input.mousePosition);
        }
    }
}
