using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TouchUnityEvent : UnityEvent<Vector3, bool> {}

public class Touchable : MonoBehaviour
{
    public bool isTouched;

    public TouchUnityEvent beginMethod;
    public TouchUnityEvent dragMethod;
    public TouchUnityEvent completeMethod;

    private void OnEnable()
    {
        TouchController.OnTouchBegin += Begin;
        TouchController.OnTouchDrag += Drag;
        TouchController.OnTouchComplete += Complete;
    }

    private void OnDisable()
    {
        TouchController.OnTouchBegin -= Begin;
        TouchController.OnTouchDrag -= Drag;
        TouchController.OnTouchComplete -= Complete;
    }

    private void Begin(Vector3 position)
    {
        beginMethod.Invoke(position, isTouched);
    }

    private void Drag(Vector3 position)
    {
        dragMethod.Invoke(position, isTouched);
    }

    private void Complete(Vector3 position)
    {
        completeMethod.Invoke(position, isTouched);
    }
}