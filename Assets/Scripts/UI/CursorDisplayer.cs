using System.Collections;
using UnityEngine;

public class CursorDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject cursorBackground;
    [SerializeField]
    private GameObject cursorPointer;

    [SerializeField]
    private float cursorReturnSpeed = 500;

    private Animator _cursorAnimator;

    private RectTransform _backgroundRectTransform;
    private RectTransform _pointerRectTransform;

    private Vector2 _backgroundPosition;
    private Vector2 _pointerPosition;
    private float _pointerBoundaryRadius;

    public Vector2 GetDragVector { get { return (_pointerPosition - _backgroundPosition) / _pointerBoundaryRadius;} }
    
    private void Awake()
    {
        _cursorAnimator = this.GetComponent<Animator>();

        _backgroundRectTransform = cursorBackground.GetComponent<RectTransform>();
        _pointerRectTransform = cursorPointer.GetComponent<RectTransform>();

        _pointerBoundaryRadius = (_backgroundRectTransform.rect.width - _pointerRectTransform.rect.width) / 2;
    }

    public void CursorDragStart()
    {
        _cursorAnimator.SetTrigger("FadeIn");
        StopAllCoroutines();
    }

    public void CursorDragEnd()
    {
        _cursorAnimator.SetTrigger("FadeOut");
        StartCoroutine(MovePointerToOrigin());
    }

    public void SetBackgroundPosition(Vector2 position)
    {
        _backgroundRectTransform.anchoredPosition = position;
        _backgroundPosition = position;
    }

    public void SetCursorPosition(Vector2 position)
    {
        Vector2 offset = Vector2.ClampMagnitude(position - _backgroundPosition, _pointerBoundaryRadius);

        _pointerPosition = _backgroundPosition + offset;
        _pointerRectTransform.anchoredPosition = _pointerPosition;

        #if UNITY_EDITOR
        Debug.DrawLine(_backgroundPosition, _pointerPosition);
        #endif
    }

    private IEnumerator MovePointerToOrigin()
    {
        while (_pointerPosition != _backgroundPosition)
        {
            _pointerPosition = _pointerRectTransform.anchoredPosition;

            _pointerRectTransform.anchoredPosition = Vector2.MoveTowards(_pointerPosition, _backgroundPosition, cursorReturnSpeed * Time.deltaTime);
            yield return null;
        }
    }
}