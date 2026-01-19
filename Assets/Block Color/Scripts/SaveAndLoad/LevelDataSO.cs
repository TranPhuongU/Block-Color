using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public int width;
    public int height;
    public int moveAmount;
    public PieceColor[] colors;

    public PieceColor targetColor;

    public int Index(int x, int y)
    {
        return y * width + x;
    }
}
