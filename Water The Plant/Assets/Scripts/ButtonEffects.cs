using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Vector3 originalScale;
    private RectTransform rectTransform;

    public float hoverScale = 1.1f;
    public float animationSpeed = 0.1f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalScale = rectTransform.localScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rectTransform != null)
            LeanTween.scale(rectTransform, originalScale * hoverScale, animationSpeed).setEaseOutQuad();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rectTransform != null)
            LeanTween.scale(rectTransform, originalScale, animationSpeed).setEaseOutQuad();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (rectTransform != null)
            LeanTween.scale(rectTransform, originalScale * hoverScale, animationSpeed).setEaseOutQuad();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (rectTransform != null)
            LeanTween.scale(rectTransform, originalScale, animationSpeed).setEaseOutQuad();
    }
}