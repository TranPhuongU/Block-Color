using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialPage
{
    public RectTransform top;
    public RectTransform content;
    public CanvasGroup cg;
}

public class UIDOManger : MonoBehaviour
{
    [SerializeField] private SwipeController swipeController;
    [SerializeField] private Image introImage;
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform tutorialButton;
    [SerializeField] private RectTransform settingButton;

    [Header("Setting panel")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private RectTransform settingPanelRect,settingButtonRect,targetTextRect,moveTextRect;
    [SerializeField] private float topPosY, middlePosY;
    [SerializeField] private float tweenDuration;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button closeSettingButton;

    [Header("Tutorial panel")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TutorialPage[] pages;
    [SerializeField] private float tweenTutorialDuration;
    [SerializeField] private CanvasGroup currentPageCanvasGroup;
    [SerializeField] private RectTransform nextButton;
    [SerializeField] private RectTransform previousButton;
    [SerializeField] private Button closeTutorialButton;

    [Header("Level panel")]
    [SerializeField] private GameObject menuLevelPanel;
    [SerializeField] private RectTransform levelPanelRect;

    [SerializeField] private bool isMenu;

    [SerializeField] private Ease ease;

    private void Awake()
    {
        Time.timeScale = 1f;
    }
    private void Start()
    {
        if(!isMenu)
            introImage.DOFade(0, 2f).SetEase(ease).SetUpdate(true);

        if (isMenu)
        {
            playButton.DOAnchorPosY(0, .5f);
            tutorialButton.DOAnchorPosY(0, .6f);
            settingButton.DOAnchorPosY(0, .7f);
        }

    }


    public async void Pause()
    {
        closeSettingButton.interactable = false;
        settingPanel.SetActive(true);
        Time.timeScale = 0;
        await PausePanelIntro();

        closeSettingButton.interactable = true;
    }

    public async void Resume()
    {
        await PausePanelOutro();
        settingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    async Task PausePanelIntro()
    {

        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        if (!isMenu)
        {
            settingButtonRect.DOAnchorPosX(300, tweenDuration).SetUpdate(true);
            targetTextRect.DOAnchorPosY(100, tweenDuration).SetUpdate(true);
            moveTextRect.DOAnchorPosX(-550, tweenDuration).SetUpdate(true);
        }
        else
        {
            playButton.DOAnchorPosX(-1500, tweenDuration).SetUpdate(true);
            tutorialButton.DOAnchorPosX(1500, tweenDuration).SetUpdate(true);
        }
        await settingPanelRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();

    }
    async Task PausePanelOutro()
    {
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await settingPanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        
        if (!isMenu)
        {
            settingButtonRect.DOAnchorPosX(0, tweenDuration).SetUpdate(true);
            targetTextRect.DOAnchorPosY(0, tweenDuration).SetUpdate(true);
            moveTextRect.DOAnchorPosX(0, tweenDuration).SetUpdate(true);
        }
        else
        {
            playButton.DOAnchorPosX(0, .3f).SetUpdate(true);
            tutorialButton.DOAnchorPosX(0, .3f).SetUpdate(true);
        }
            
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0;
        TutorialIntro();

    }

    public async void HideTutorial()
    {
        await TutorialOutro();
        ResetAllTutorialPages();

        tutorialPanel.SetActive(false);
        Time.timeScale = 1;
    }
    async Task TutorialOutro()
    {
        
        canvasGroup.DOFade(0, tweenTutorialDuration).SetUpdate(true);
        nextButton.DOAnchorPosX(800, tweenDuration).SetUpdate(true);
        previousButton.DOAnchorPosX(-800, tweenDuration).SetUpdate(true);
        currentPageCanvasGroup.DOFade(0, tweenDuration).SetUpdate(true);

        int index = swipeController.currentPage - 1;
        if (index < 0 || index >= pages.Length) return;

        var page = pages[index];

        page.cg.DOFade(0, .1f).SetUpdate(true);

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(page.content.DOSizeDelta(new Vector2(2170.4f, 0), tweenTutorialDuration));

        seq.Append(page.top.DOSizeDelta(Vector2.zero, .3f));

        await seq.AsyncWaitForCompletion();

        if (isMenu)
        {
            playButton.DOAnchorPosX(0, .3f).SetUpdate(true);
            settingButton.DOAnchorPosX(0, .3f).SetUpdate(true);
        }

    }

    void TutorialIntro()
    {
        closeTutorialButton.interactable = false;
        if (isMenu)
        {
            playButton.DOAnchorPosX(1500, tweenDuration).SetUpdate(true);
            settingButton.DOAnchorPosX(-1500, tweenDuration).SetUpdate(true);
        }

        canvasGroup.DOFade(1, tweenTutorialDuration).SetUpdate(true);
        nextButton.DOAnchorPosX(0, tweenDuration).SetUpdate(true);
        previousButton.DOAnchorPosX(0, tweenDuration).SetUpdate(true);
        currentPageCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true);


        var page = pages[0];
        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(page.top.DOSizeDelta(new Vector2(2170.4f, 50), .3f));
        seq.Append(page.content.DOSizeDelta(new Vector2(2170.4f, 500), tweenTutorialDuration));
        seq.Append(page.cg.DOFade(1, .2f).SetUpdate(true));

        var page1 = pages[1];
        Sequence seq1 = DOTween.Sequence().SetUpdate(true);

        seq1.Append(page1.top.DOSizeDelta(new Vector2(2170.4f, 50), .3f));
        seq1.Append(page1.content.DOSizeDelta(new Vector2(2170.4f, 500), tweenTutorialDuration));
        seq1.Append(page1.cg.DOFade(1, .2f).SetUpdate(true));

        DOTween.Sequence().Append(seq).OnComplete(() =>
        {
            closeTutorialButton.interactable = true;

        }).SetUpdate(true);
     
    }
    void ResetAllTutorialPages()
    {
        // top đóng
        pages[0].top.sizeDelta = Vector2.zero;

        // content đóng (đúng width, height = 0)
        pages[0].content.sizeDelta = new Vector2(2170.4f, 0);

        // text ẩn
        pages[0].cg.alpha = 0;
      
    }

    public void ShowLevelPanel()
    {
        menuLevelPanel.SetActive(true);
        LevelIntro();

    }

    public async void HideLevelPanel()
    {
        await LevelOutro();
        menuLevelPanel.SetActive(false);
    }
    void LevelIntro()
    {

        tutorialButton.DOAnchorPosX(1500, tweenDuration).SetUpdate(true);
        settingButton.DOAnchorPosX(-1500, tweenDuration).SetUpdate(true);

        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        levelPanelRect.GetComponent<CanvasGroup>().DOFade(1, tweenDuration).SetUpdate(true);
        levelPanelRect.DOScale(new Vector3(1, 1, 1), tweenDuration).SetUpdate(true);
    }
    async Task LevelOutro()
    {

        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        levelPanelRect.GetComponent<CanvasGroup>().DOFade(0, tweenDuration).SetUpdate(true);
        await levelPanelRect.DOScale(new Vector3(0, 0, 0), tweenDuration).SetUpdate(true).AsyncWaitForCompletion(); 
        tutorialButton.DOAnchorPosX(0, .3f).SetUpdate(true);
        settingButton.DOAnchorPosX(0, .3f).SetUpdate(true);
    }

}
