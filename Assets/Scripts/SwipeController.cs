using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    [SerializeField] private Image[] barImage;
    [SerializeField] private Sprite barClosed, barOpen;

    [SerializeField] private RectTransform canvasRect;

    private Vector3 startTransform;

    float dragThreshould;
    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        startTransform = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15f;
        //pageStep = new Vector3(-canvasRect.rect.width, 0, 0);

        UpdateBar();
    }
    public void Next()
    {
        if(currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);

        UpdateBar();
    }

    public void ResetToFirstPage()
    {
        currentPage = 1;

        targetPos = startTransform;
        levelPagesRect.localPosition = startTransform;

        UpdateBar();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();

        }
        else
        {
            MovePage();
        }
    }

    void UpdateBar()
    {
        foreach (var item in barImage)
        {
            item.sprite = barClosed;
        }

        barImage[currentPage - 1].sprite = barOpen;
    }
}
