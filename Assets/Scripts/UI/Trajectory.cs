using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    private List<Vector3> _points = new List<Vector3>();

    public List<Vector3> Points { get => _points; set => _points = value; }

    public Animator _animator;

    public void Draw()
    {
        lineRenderer.positionCount = _points.Count;
        lineRenderer.SetPositions(_points.ToArray());
    }

    public void ClearPoints()
    {
        _points.Clear();
    }

    public void Reset()
    {
        ClearPoints();
        Draw();
    }

    public void FadeIn()
    {
        _animator.ResetTrigger("FadeOut");
        _animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {   
        _animator.ResetTrigger("FadeIn");
        _animator.SetTrigger("FadeOut");
    }

    public Vector3[] ProcessTrajectory()
    {
        _points.RemoveAt(0);
        var points = _points.ToArray();

        return points;
    }
}
