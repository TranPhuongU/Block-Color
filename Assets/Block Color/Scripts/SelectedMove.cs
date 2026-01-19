using System.Collections;
using UnityEngine;

public class SelectedMove : MonoBehaviour
{
    private RectTransform rect;

    public float startX = 160f;
    public float targetX = -120f;
    public float moveDuration = 0.5f; // thời gian trượt (giây)

    void Start()
    {
        rect = GetComponent<RectTransform>();

        // Lấy Y hiện tại (giữ nguyên)
        float y = rect.anchoredPosition.y;

        // Set vị trí ban đầu
        rect.anchoredPosition = new Vector2(startX, y);

        // Bắt đầu animation
        StartCoroutine(MoveToTarget(y));
    }

    IEnumerator MoveToTarget(float y)
    {
        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;

            float x = Mathf.Lerp(startX, targetX, t);
            rect.anchoredPosition = new Vector2(x, y);

            yield return null;
        }

        // Đảm bảo snap đúng vị trí cuối
        rect.anchoredPosition = new Vector2(targetX, y);
    }
}
