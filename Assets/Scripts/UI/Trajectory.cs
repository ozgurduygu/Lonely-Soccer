using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private List<Vector3> _points = new List<Vector3>();

    public Vector3 lastPoint { get { return _points.Last(); } }

    public void Draw()
    {   
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.SetPositions(_points.ToArray());
    }

    public void Clear()
    {
        _points.Clear();
        Draw();
    }

    public void AddPoint(Vector3 v)
    {
        _points.Add(v);
    }

    private void Update()
    {
        _lineRenderer.material.SetTextureOffset("_MainTex", Vector2.left * Time.time);
    }
}
