using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SelectColor : MonoBehaviour, IPointerClickHandler
{
    public PieceColor colorSelect;
    public GameObject activeFrame;

    private void Start()
    {
        if(colorSelect == PieceColor.Blue)
        {
            GameManager.instance.currentSelect = this;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.instance.GetGameState() != GameState.Gameplay)
            return;

        SoundManager.instance.PlayButtonSound();

        if(GameManager.instance.currentSelect == this)
            return;

        Active();
    }

    void Active()
    {
        if(GameManager.instance.currentSelect != null)
        {
            GameManager.instance.currentSelect.activeFrame.SetActive(false);
        }

        activeFrame.SetActive(true);

        GameManager.instance.currentSelect = this;
        GameManager.colorSelect = colorSelect;
    }
}
