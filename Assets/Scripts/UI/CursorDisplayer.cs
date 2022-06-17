using System.Collections;
using UnityEngine;

public class CursorDisplayer : MonoBehaviour
{   
    [SerializeField] private float cursorReturnSpeed = 500;

    [SerializeField] private Animator cursorAnimator;

    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private RectTransform pointerRectTransform;

    private Vector2 _backgroundPosition;
    private Vector2 _pointerPosition;
    private float _pointerBoundaryRadius;

    public Vector2 Value
    { 
        get => (_pointerPosition - _backgroundPosition) / _pointerBoundaryRadius;
    }
    
    private void Start()
    {
        _pointerBoundaryRadius = (backgroundRectTransform.rect.width - pointerRectTransform.rect.width) / 2;
    }

    public void CursorDragStart()
    {
        cursorAnimator.SetTrigger("FadeIn");
        StopAllCoroutines();
    }

    public void CursorDragEnd()
    {
        cursorAnimator.SetTrigger("FadeOut");
        StartCoroutine(MovePointerToOriginCoroutine());
    }

    public void SetBackgroundPosition(Vector2 position)
    {
        backgroundRectTransform.anchoredPosition = position;
        _backgroundPosition = position;
    }

    public void SetCursorPosition(Vector2 position)
    {
        var offset = Vector2.ClampMagnitude(position - _backgroundPosition, _pointerBoundaryRadius);

        _pointerPosition = _backgroundPosition + offset;
        pointerRectTransform.anchoredPosition = _pointerPosition;

        #if UNITY_EDITOR
        Debug.DrawLine(_backgroundPosition, _pointerPosition);
        #endif
    }

    private IEnumerator MovePointerToOriginCoroutine()
    {
        while (_pointerPosition != _backgroundPosition)
        {
            _pointerPosition = pointerRectTransform.anchoredPosition;
            
            pointerRectTransform.anchoredPosition = Vector2.MoveTowards(_pointerPosition, _backgroundPosition, cursorReturnSpeed * Time.deltaTime);
            yield return null;
        }
    }
}