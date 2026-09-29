using UnityEngine;

public class ChessPieceView : MonoBehaviour
{
    public void UpdatePosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, 0, position.y);
    }
}
