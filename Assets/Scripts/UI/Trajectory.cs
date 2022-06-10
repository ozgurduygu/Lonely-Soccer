using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private Vector3[] points = new Vector3[0];

    public Vector3 lastPoint { get { return points[points.Length - 1]; } }

    public void Draw()
    {
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }

    public void Clear()
    {
        _lineRenderer.positionCount = 0;
        points = null;
        _lineRenderer.SetPositions(points);
    }

    public void AddPoint(Vector3 v)
    {
        Debug.Log(points);
        var newPoints = new Vector3[points.Length + 1];

        for (int i = 0; i < points.Length; i++)
        {
            newPoints[i] = points[i];
        }

        newPoints[newPoints.Length - 1] = v;
        points = newPoints;
    }
}
