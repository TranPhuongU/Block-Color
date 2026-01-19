using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class FrameBoardSize : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private Transform cameraPosition;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        
    }
    public void SetupFrame()
    {
        int boardWidth = GameManager.instance.board.width;
        int boardHeight = GameManager.instance.board.height;

        sprite.size = new Vector2(boardWidth, boardHeight);

        transform.position = new Vector3((boardWidth - 1) / 2f, (boardHeight - 1) / 2f, 0);
    }
}
