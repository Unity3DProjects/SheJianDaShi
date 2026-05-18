using UnityEngine;

[DisallowMultipleComponent]
public class ScrollByTouch : MonoBehaviour
{
    private RectTransform contentTrans;

    private void Awake()
    {
        contentTrans = GetComponent<RectTransform>();
    }

    private float minSwipeDistance = 0.001f; // 最小滑动距离    

    private void Update()
    {
        // 确保有触摸输入  
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    break;

                case TouchPhase.Moved:
                    DetectSwipe(touch.deltaPosition);
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }

    private void DetectSwipe(Vector2 delta)
    {
        float distance = delta.y;

        if (Mathf.Abs(distance) >= minSwipeDistance)
        {
            var rectTrans = contentTrans as RectTransform;

            var vector2 = rectTrans.anchoredPosition;
            vector2 =
                new Vector2(vector2.x, vector2.y + distance * 2);
            rectTrans.anchoredPosition = vector2;
        }
    }
}