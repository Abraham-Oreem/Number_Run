using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnTouchStart;
    public static event Action<Vector2> OnDrag;
    public static event Action<Vector2> OnTouchEnd;

    private static Vector2 startPosition;
    private static bool isDragging;

    void Update()
    {
        HandleTouch();
    }


    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                startPosition = touch.position;
                OnTouchStart?.Invoke(startPosition);
                break;

            case TouchPhase.Moved:
                OnDrag?.Invoke(touch.deltaPosition);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                OnTouchEnd?.Invoke(touch.position);
                break;
        }
    }
}
