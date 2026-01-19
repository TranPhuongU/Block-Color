
using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public enum GameState
{
    Intro,
    Gameplay,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameState gameState;

    public static PieceColor colorSelect;

    [SerializeField] private LevelDatabaseSO database;

    public Board board;
    [SerializeField] private TargetColorDropdown targetColorDropdown;

    public TMP_InputField levelNameInput;
    public TMP_InputField moveAmountInput;

    private int currentLevelIndex;

    public CreateModeSO createModeSO;

    public SelectColor currentSelect { get; set; }

    public Action<GameState> onGameStateCallBack;

    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 90;

    }

    private void Start()
    {
        colorSelect = PieceColor.Blue;
        gameState = GameState.Intro;
    }

    public void GameStateCallBack(GameState _gameState)
    {

        gameState = _gameState;
        onGameStateCallBack?.Invoke(gameState);
    }

    public GameState GetGameState() => gameState;
    public void SaveProgress()
    {
        ProgressManager.UnlockNext(currentLevelIndex, database);
    }
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= database.allLevels.Length)
        {
            Debug.LogError($"Level index không hợp lệ: {index}");
            return;
        }

        currentLevelIndex = index;

        LevelDataSO data = database.allLevels[index];

        board.currentLevel = data;
        board.moveAmount = data.moveAmount;
        board.targetColor = data.targetColor;

        board.width = data.width;
        board.height = data.height;

    }
    public LevelDatabaseSO GetLevelDB() => database;
    public void SaveLevelAsScriptableObject()
    {
#if UNITY_EDITOR
        string levelName = levelNameInput.text.Trim();

        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogWarning("Level name cannot be empty!");
            return;
        }

        // 🔍 VALIDATE MOVE AMOUNT
        string moveText = moveAmountInput.text.Trim();

        if (!int.TryParse(moveText, out int moveAmount))
        {
            Debug.LogWarning("Move Amount must be a number!");
            return;
        }

        if (moveAmount <= 0)
        {
            Debug.LogWarning("Move Amount must be greater than 0!");
            return;
        }

        LevelDataSO level = ScriptableObject.CreateInstance<LevelDataSO>();

        level.width = board.width;
        level.height = board.height;
        level.moveAmount = moveAmount; // ✅ gán sau khi validate
        level.targetColor = targetColorDropdown.SelectedColor;

        level.colors = new PieceColor[board.width * board.height];

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                level.colors[level.Index(x, y)] =
                    board.allPiece[x, y].color;
            }
        }

        string folderPath = "Assets/Data/Levels";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/Data/Levels", "Levels");
        }

        string assetPath = $"{folderPath}/Level_{levelName}.asset";

        AssetDatabase.CreateAsset(level, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Level saved: " + assetPath);
#endif
    }

}

