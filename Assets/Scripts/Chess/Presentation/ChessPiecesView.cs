using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiecesView : MonoBehaviour
{
    [SerializeField]
    private GameObject _WhitePawnPrefab;
    [SerializeField]
    private GameObject _WhiteRookPrefab;
    [SerializeField]
    private GameObject _WhiteKnightPrefab;
    [SerializeField]
    private GameObject _WhiteBishopPrefab;
    [SerializeField]
    private GameObject _WhiteQueenPrefab;
    [SerializeField]
    private GameObject _WhiteKingPrefab;
    [SerializeField]
    private GameObject _BlackPawnPrefab;
    [SerializeField]
    private GameObject _BlackRookPrefab;
    [SerializeField]
    private GameObject _BlackKnightPrefab;
    [SerializeField]
    private GameObject _BlackBishopPrefab;
    [SerializeField]
    private GameObject _BlackQueenPrefab;
    [SerializeField]
    private GameObject _BlackKingPrefab;

    private Dictionary<(Type, ChessColor), GameObject> _piecePrefabs;
    private Dictionary<ChessPiece, ChessPieceView> _pieceViews;

    private void Awake()
    {
        _piecePrefabs = new()
        {
            {(typeof(Pawn), ChessColor.White), _WhitePawnPrefab},
            {(typeof(Rook), ChessColor.White), _WhiteRookPrefab},
            {(typeof(Knight), ChessColor.White), _WhiteKnightPrefab},
            {(typeof(Bishop), ChessColor.White), _WhiteBishopPrefab},
            {(typeof(Queen), ChessColor.White), _WhiteQueenPrefab},
            {(typeof(King), ChessColor.White), _WhiteKingPrefab},
            {(typeof(Pawn), ChessColor.Black), _BlackPawnPrefab},
            {(typeof(Rook), ChessColor.Black), _BlackRookPrefab},
            {(typeof(Knight), ChessColor.Black), _BlackKnightPrefab},
            {(typeof(Bishop), ChessColor.Black), _BlackBishopPrefab},
            {(typeof(Queen), ChessColor.Black), _BlackQueenPrefab},
            {(typeof(King), ChessColor.Black), _BlackKingPrefab}
        };

        _pieceViews = new();
    }

    public void Render(ChessBoard board)
    {
        foreach (ChessPiece piece in board.GetPieces())
        {
            //GameObject prefab = _piecePrefabs[(piece.GetType(), piece.Color)];
            Vector3 position = new Vector3(piece.Position.x, 0, piece.Position.y);

            if (_piecePrefabs.TryGetValue((piece.GetType(), piece.Color), out GameObject prefab))
            {
                GameObject pieceObject = Instantiate(prefab, position, prefab.transform.rotation, transform);

                ChessPieceView pieceView = pieceObject.GetComponent<ChessPieceView>();
                _pieceViews.Add(piece, pieceView);
            }
            else
            {
                Debug.LogError($"No prefab found for {piece.GetType()} {piece.Color}");
            }
        }
    }

    public void UpdatePiecePosition(ChessPiece piece, Vector2Int position)
    {
        if(_pieceViews.TryGetValue(piece, out ChessPieceView pieceView))
        {
            pieceView.UpdatePosition(position);
        }
    }

    public void RemovePiece (ChessPiece piece)
    {
        if (_pieceViews.TryGetValue(piece, out ChessPieceView pieceView))
        {
            Destroy(pieceView.gameObject);
            _pieceViews.Remove(piece);
        }
    }

    public void ReplacePieceView(ChessPiece oldPiece, ChessPiece newPiece, Vector2Int position)
    {
        RemovePiece(oldPiece);

        if(_piecePrefabs.TryGetValue((newPiece.GetType(), newPiece.Color), out GameObject prefab))
        {
            Vector3 pos = new Vector3(position.x, 0, position.y);
            GameObject pieceObject = Instantiate(prefab, pos, prefab.transform.rotation, transform);

            ChessPieceView pieceView = pieceObject.GetComponent<ChessPieceView>();
            _pieceViews.Add(newPiece, pieceView);
        }
        else
        {
            Debug.LogError($"No prefab found for {newPiece.GetType()} {newPiece.Color}");
        }
    }
}
