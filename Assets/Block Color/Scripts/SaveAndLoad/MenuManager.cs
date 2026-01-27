using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private Image darkScreenFader;

    [SerializeField] private CreateModeSO createModeSO;

    [SerializeField] private Ease ease;

    public LevelDatabaseSO database;

    private bool isInitialized = false; // 👈 CHỈ THÊM DÒNG NÀY
    private void Awake()
    {
        Application.targetFrameRate = 90;

    }
    public void ActiveLevelPanel()
    {
        if (isInitialized)
            return; // 👈 nếu đã spawn rồi thì dừng ở đây

        isInitialized = true;

        GameProgress progress = ProgressManager.Load(database);
        int total = database.allLevels.Length;

        for (int i = 0; i < total; i++)
        {
            int levelIndex = i;
            int levelID = i + 1;

            LevelProgress dataSave = progress.levels[i];

            LevelButton btn =
                Instantiate(levelButtonPrefab, buttonContainer)
                .GetComponent<LevelButton>();

            btn.SetLevelNumber(levelID);
            btn.UpdateView(dataSave);

            btn.SetClickAction(() =>
            {
                createModeSO.createMode = false;
                SelectedLevel.levelID = levelIndex;
                SoundManager.instance.PlayButtonSound();

                darkScreenFader.DOFade(1f, 1f).SetEase(ease).SetUpdate(true).OnComplete(() =>
                {
                    SceneManager.LoadScene("Gameplay");
                });
            });
        }
    }

    public void PlayCreateMode()
    {
        createModeSO.createMode = true;
        SceneManager.LoadScene("Gameplay");
    }

    [ContextMenu("Delete Save Data")]
    private void DeleteSaveData()
    {
        ProgressManager.DeleteSave();
        Debug.Log("Save data đã bị xóa");
    }
}


public static class SelectedLevel
{
    public static int levelID;
}
