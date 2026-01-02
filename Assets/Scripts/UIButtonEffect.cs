using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonEffect : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale (PC Hover)")]
    [SerializeField] private float hoverScale = 1.25f;
    [SerializeField] private float scaleSpeed = 10f;

    [Header("Color (Mobile Press)")]
    [SerializeField] private float brightenAmount = 1.15f;

    private Vector3 originalScale;
    private Image image;
    private Color originalColor;

    private Vector3 targetScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        image = GetComponent<Image>();
        if (image != null)
            originalColor = image.color;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        // PC: scale mượt
        transform.localScale =
            Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
#endif
    }

    // ===== PC: Hover =====
    public void OnPointerEnter(PointerEventData eventData)
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        targetScale = originalScale * hoverScale;
#endif
    }

    public void OnPointerExit(PointerEventData eventData)
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        targetScale = originalScale;
#endif
    }

    // ===== Mobile: Press =====
    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (image != null)
        {
            image.color = originalColor * brightenAmount;
        }
#endif
    }

    public void OnPointerUp(PointerEventData eventData)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (image != null)
        {
            image.color = originalColor;
        }
#endif
    }
}
