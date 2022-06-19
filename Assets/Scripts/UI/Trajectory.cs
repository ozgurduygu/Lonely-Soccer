using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LineRenderer lineRenderer;

    private List<Vector3> _points = new List<Vector3>();

    public List<Vector3> Points
    {
        get => _points;
        set => _points = value;
    }


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
        animator.ResetTrigger("FadeOut");
        animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        animator.ResetTrigger("FadeIn");
        animator.SetTrigger("FadeOut");
    }

    public Vector3[] ProcessTrajectory()
    {
        // Ignore starting point.
        _points.RemoveAt(0);

        return _points.ToArray();
    }
}
