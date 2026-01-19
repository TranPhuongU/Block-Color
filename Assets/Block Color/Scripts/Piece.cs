using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceColor
{
    Blue,
    Red,
    Yellow,
    Green,
    None

}
public class Piece : MonoBehaviour
{
    public PieceColor color;

    public Color[] spriteColors;
    public SpriteRenderer sprite {  get;  set; }

    public bool check;
    public int x;
    public int y;

    public Color originalColor {  get; set; }

    private Coroutine animRoutine;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

    }

    public void SetupPiece(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void SetupColor(PieceColor pieceColor)
    {
        color = pieceColor;

        Color c = ConvertToUnityColor(pieceColor);
        sprite.color = new Color(c.r, c.g, c.b, 0f);

        // reset scale nhỏ
        transform.localScale = Vector3.one * 0.6f;

        // chạy animation
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(ScaleAndFadeIn(c));
    }
    private IEnumerator ScaleAndFadeIn(Color targetColor)
    {
        float duration = 0.25f;
        float t = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            float scaleEase = Mathf.SmoothStep(0f, 1f, t);

            // 🔥 ALPHA CHẬM HƠN SCALE
            float alphaEase = Mathf.SmoothStep(0f, 1f, t * 0.7f);

            transform.localScale = Vector3.Lerp(startScale, endScale, scaleEase);

            sprite.color = new Color(
                targetColor.r,
                targetColor.g,
                targetColor.b,
                alphaEase
            );

            yield return null;
        }

        transform.localScale = endScale;
        sprite.color = targetColor;
        originalColor = sprite.color;
    }



    Color ConvertToUnityColor(PieceColor pieceColor)
    {
        switch (pieceColor)
        {
            case PieceColor.Blue: return spriteColors[0];
            case PieceColor.Red: return spriteColors[1];
            case PieceColor.Yellow: return spriteColors[2];
            case PieceColor.Green: return spriteColors[3];
            case PieceColor.None: return spriteColors[4];
            default: return Color.white;
        }
    }


}
