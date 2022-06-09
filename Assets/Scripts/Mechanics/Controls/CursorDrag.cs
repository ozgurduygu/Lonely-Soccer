using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDrag : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float minDragMagnitude = .15f;

    private BallPhysics _ballPhysics;
    private CursorDisplayer _cursorDisplayer;

    private bool _isDragging;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private Vector3 _ballSpawnPosition;

    private void Awake()
    {
        _cursorDisplayer = GetComponent<CursorDisplayer>();
        _ballPhysics = ball.GetComponent<BallPhysics>();

        _ballSpawnPosition = ball.transform.position;
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
            _cursorDisplayer.CursorDragEnd();
            
            if(_cursorDisplayer.Value.magnitude < minDragMagnitude)
            {
                
            }
        }

        if (_isDragging)
        {
            _cursorDisplayer.SetCursorPosition(Input.mousePosition);
            
            var initialVelocity = new Vector3(_cursorDisplayer.Value.x, 0, _cursorDisplayer.Value.y) * -40f;
            _ballPhysics.CalculateTrajectory(initialVelocity);
        }
    }
}
