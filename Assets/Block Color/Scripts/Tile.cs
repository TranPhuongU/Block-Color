using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private Board board;

    public int x;
    public int y;

    private void Start()
    {
        board = GameManager.instance.board;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(board.isResolving || GameManager.instance.GetGameState() != GameState.Gameplay)
            return;

        CheckAndReplaceColor();
    }

    private void CheckAndReplaceColor()
    {
        board.StartCoroutineReplace(x, y);
    }

    public void SetupTile(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!GameManager.instance.createModeSO.createMode && GameManager.instance.GetGameState() == GameState.Gameplay && !board.isResolving)
            board.MouseEnterPiece(x, y);

        if (GameManager.instance.createModeSO.createMode && board.isDragging)
        {
            CheckAndReplaceColor();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!GameManager.instance.createModeSO.createMode)
            return;

        board.isDragging = true;
        CheckAndReplaceColor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GameManager.instance.createModeSO.createMode)
            return;

        board.isDragging = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GameManager.instance.createModeSO.createMode && GameManager.instance.GetGameState() == GameState.Gameplay && !board.isResolving)
            board.MouseExitPiece(x, y);
    }
}
