using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public struct Grid
{
    int x;
    int y;
}
public class Board : MonoBehaviour
{
    public static Board instance;

    public PieceColor targetColor {  get; set; }

    public LevelDataSO currentLevel {  get; set; }

    public float borderSize;

    public int width = 10;
    public int height = 8;

    public int currentMoveAmount {  get; set; }
    public int moveAmount {  get; set; }

    public GameObject tilePrefab;
    public GameObject piecePrefab;

    public Piece[,] allPiece {  get; set; }

    public bool isDragging {  get; set; }

    public bool isResolving;

    [SerializeField] private FrameBoardSize frameBoardSize;
    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {

        if (!GameManager.instance.createModeSO.createMode)
        {
            GameManager.instance.LoadLevel(SelectedLevel.levelID);

        }
        else
        {
            width = GameManager.instance.createModeSO.width;
            height = GameManager.instance.createModeSO.height;

        }

        

        allPiece = new Piece[width,height];

        currentMoveAmount = moveAmount;

        SetupCamera();

        SetupTile();

        StartCoroutine(SpawnPieceRoutine());
    }
    [SerializeField] private float cameraYOffset = .3f;

    void SetupCamera()
    {
        Camera.main.transform.position =
            new Vector3(
                (width - 1) / 2f,
                (height - 1) / 2f + cameraYOffset,
                -10f
            );

        float aspectRatio = (float)Screen.width / Screen.height;

        float verticalSize = height / 2f + borderSize;
        float horizontalSize = (width / 2f + borderSize) / aspectRatio;

        Camera.main.orthographicSize =
            (verticalSize > horizontalSize) ? verticalSize : horizontalSize;

        frameBoardSize.SetupFrame();
    }



    private void SetupTile()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity, transform);
                newTile.GetComponent<Tile>().SetupTile(i, j);
                newTile.gameObject.name = "Tile" + "[" + i + ", " + j + "]";
            }
        }
    }
    private IEnumerator SpawnPieceRoutine()
    {
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject newPiece =
                    Instantiate(piecePrefab, new Vector2(x, y), Quaternion.identity, transform);

                newPiece.name = $"Piece[{x},{y}]";

                Piece piece = newPiece.GetComponent<Piece>();
                piece.SetupPiece(x, y);
                allPiece[x, y] = piece;

                if (!GameManager.instance.createModeSO.createMode)
                {
                    int index = currentLevel.Index(x, y);
                    piece.SetupColor(currentLevel.colors[index]);
                }

                // 🔑 CHỜ ĐÚNG 1 FRAME RỒI MỚI SPAWN PIECE TIẾP
                yield return null;
            }
        }

        GameManager.instance.GameStateCallBack(GameState.Gameplay);
    }

    public void ReloadButton()
    {
        if(isResolving)
            return;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Piece piece = allPiece[i, j] ;

                if (!GameManager.instance.createModeSO.createMode)
                {
                    int index = currentLevel.Index(i, j);
                    piece.SetupColor(currentLevel.colors[index]);

                    currentMoveAmount = moveAmount;
                    UIManager.instance.UpdateMoveText(currentMoveAmount);
                }
            }
        }
    }
    private void SetupPiece()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject newPiece = Instantiate(piecePrefab, new Vector2(i, j), Quaternion.identity, transform);
                newPiece.gameObject.name = "Piece" + "[" + i + ", " + j + "]";
                Piece piece = newPiece.GetComponent<Piece>();
                piece.SetupPiece(i, j);
                allPiece[i, j] = piece;

                if (GameManager.instance.createModeSO.createMode)
                {
                    continue;
                }
                int index = currentLevel.Index(i, j);

                PieceColor color = currentLevel.colors[index];

                allPiece[i, j].SetupColor(color);
            }
        }
    }


    public IEnumerator ReplaceColorCoroutine(
    int startX,
    int startY,
    PieceColor sourceColor,
    PieceColor resolveColor
)
    {
        isResolving = true;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));

        float startDelay = 0.3f;   // delay lượt đầu
        float minDelay = 0.06f;   // delay nhỏ nhất
        float delayDecay = 0.9f;   // mỗi lượt nhanh hơn 15%

        float currentDelay = startDelay;

        int waveIndex = 0;

        while (queue.Count > 0)
        {
            int waveSize = queue.Count; // SỐ Ô TRONG LƯỢT NÀY

            SoundManager.instance.PlayClearPieceSound(waveIndex);

            for (int i = 0; i < waveSize; i++)
            {
                Vector2Int pos = queue.Dequeue();
                int x = pos.x;
                int y = pos.y;

                if (x < 0 || x >= width || y < 0 || y >= height)
                    continue;

                Piece piece = allPiece[x, y];

                if (piece.check)
                    continue;

                if (piece.color != sourceColor)
                    continue;

                piece.check = true;
                piece.SetupColor(resolveColor);

                if (GameManager.instance.createModeSO.createMode)
                    continue;

                // enqueue hàng xóm cho lượt SAU
                queue.Enqueue(new Vector2Int(x + 1, y));
                queue.Enqueue(new Vector2Int(x - 1, y));
                queue.Enqueue(new Vector2Int(x, y + 1));
                queue.Enqueue(new Vector2Int(x, y - 1));
            }

            yield return new WaitForSeconds(currentDelay);

            currentDelay = Mathf.Max(minDelay, currentDelay * delayDecay);
            waveIndex++;
        }

        isResolving = false;
    }


    private bool CheckWinGame()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allPiece[i, j].color != targetColor)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public IEnumerator ReplaceColorRoutine(int x, int y)
    {
        ResetPieceCheck();

        PieceColor sourceColor = allPiece[x, y].color;

        PieceColor resolveColor = GameManager.colorSelect;

        yield return ReplaceColorCoroutine(x, y, sourceColor, resolveColor);

        CheckEndGame();
    }

    private void CheckEndGame()
    {
        if(GameManager.instance.createModeSO.createMode)
            return;
        currentMoveAmount--;
        UIManager.instance.UpdateMoveText(currentMoveAmount);

        if (CheckWinGame())
        {
            GameManager.instance.SaveProgress();

            int maxLevel = GameManager.instance.GetLevelDB().allLevels.Length;

            SelectedLevel.levelID++;

            if (SelectedLevel.levelID >= maxLevel)
            {
                SelectedLevel.levelID = 0;
            }

            GameManager.instance.GameStateCallBack(GameState.Win);

            FXManager.instance.PlayRandomWinEffect();

            SoundManager.instance.PlayWinSound();

            UIManager.instance.EndGamePanelActive(true);

            return;
        }
     
        if(currentMoveAmount <= 0)
        {
            GameManager.instance.GameStateCallBack(GameState.Lose);

            SoundManager.instance.PlayLoseSound();

            UIManager.instance.EndGamePanelActive(false);
        }
    }

    public void StartCoroutineReplace(int x, int y)
    {
        Piece piece = allPiece[x, y];

        if (piece.color == GameManager.colorSelect)
            return;

        StartCoroutine(ReplaceColorRoutine(x, y));
    }

    public void MouseEnterPiece(int x, int y)
    {
        Piece piece = allPiece[x, y];

        Color c = piece.originalColor;
        c.r *= 0.85f;
        c.g *= 0.85f;
        c.b *= 0.85f;

        piece.sprite.color = c;
    }


    public void MouseExitPiece(int x, int y)
    {
        Piece piece = allPiece[x, y];

        piece.sprite.color = piece.originalColor;
    }



    public void ResetPieceCheck()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Piece piece = allPiece[i, j];

                piece.check = false;
            }
        }
    }


}

