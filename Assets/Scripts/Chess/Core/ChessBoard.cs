using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard
{
    public ChessPiece?[,] Board { get; private set; }

    public ChessBoard()
    {
        Board = new ChessPiece?[8, 8];
    }

    public bool AddPiece(ChessPiece piece, Vector2Int position)
    {
        if (Board[position.x, position.y] != null)
            return false;

        Board[position.x, position.y] = piece;
        return true;
    }

    public ChessPiece? GetPiece(Vector2Int position)
    {
        return Board[position.x, position.y];
    }

    public ChessPiece? RemovePiece(Vector2Int position)
    {
        ChessPiece? chessPiece = GetPiece(position);
        if (chessPiece == null)
            return null;

        Board[position.x, position.y] = null;
        return chessPiece;
    }

    public King FindKing(ChessColor color)
    {
        foreach (ChessPiece? piece in Board)
        {
            if (piece is King king && piece.Color == color)
                return king;
        }

        throw new InvalidOperationException("King not found.");
    }

    public List<ChessPiece> GetPiecesByColor(ChessColor color)
    {
        List<ChessPiece> pieces = new List<ChessPiece>();

        foreach (ChessPiece? piece in Board)
        {
            if (piece != null && piece.Color == color)
            {
                pieces.Add(piece);
            }
        }

        return pieces;
    }

    public List<ChessPiece> GetPieces()
    {
        List<ChessPiece> pieces = new List<ChessPiece>();

        foreach (ChessPiece? piece in Board)
        {
            if (piece != null)
                pieces.Add(piece);
        }

        return pieces;
    }
}