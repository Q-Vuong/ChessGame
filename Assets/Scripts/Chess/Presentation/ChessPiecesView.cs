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
    [SerializeField]
    private int _squareSize;

    private Dictionary<(Type, ChessColor), GameObject> _piecePrefabs;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Render(ChessBoard board)
    {
        foreach (ChessPiece piece in board.GetPieces())
        {
            //GameObject prefab = _piecePrefabs[(piece.GetType(), piece.Color)];
            Vector3 position = new Vector3(piece.Position.x * _squareSize, 0, piece.Position.y * _squareSize);

            if (_piecePrefabs.TryGetValue((piece.GetType(), piece.Color), out GameObject prefab))
            {
                Instantiate(prefab, position, prefab.transform.rotation, transform);
            }
            else
            {
                Debug.LogError($"No prefab found for {piece.GetType()} {piece.Color}");
            }
        }
    }
}
