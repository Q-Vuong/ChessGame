using UnityEngine;

public class MoveValidator
{

    public bool IsValidMove(Move move, ChessBoard board)
    {
        ChessPiece? destination = board.GetPiece(move.To);
        if (destination == null)
        {
            if (!move.Piece.CanMove(move.From, move.To))
                return false;
        }
        else
        {
            if (destination.Color == move.Piece.Color)
            {
                return false;
            }
            else
            {
                if (!move.Piece.CanAttack(move.From, move.To))
                    return false;
            }
        }

        if (move.Piece.NeedsPathCheck)
        {
            if (!IsPathClear(move.From, move.To, board))
                return false;
        }

        return true;
    }

    private bool IsPathClear(Vector2Int from, Vector2Int to, ChessBoard board)
    {
        int dx = to.x - from.x;
        int dy = to.y - from.y;

        int directionX = (int)Mathf.Sign(dx);
        int directionY = (int)Mathf.Sign(dy);

        if (dx == 0 && dy != 0)
        {
            for (int y = from.y + directionY; y != to.y; y += directionY)
            {
                if (board.GetPiece(new Vector2Int(from.x, y)) != null)
                    return false;
            }
        }
        else if (dx != 0 && dy == 0)
        {
            for (int x = from.x + directionX; x != to.x; x += directionX)
            {
                if (board.GetPiece(new Vector2Int(x, from.y)) != null)
                    return false;
            }
        }
        else if (Mathf.Abs(dx) == Mathf.Abs(dy))
        {
            for (Vector2Int x = from + new Vector2Int(directionX, directionY); x != to; x += new Vector2Int(directionX, directionY))
            {
                if (board.GetPiece(x) != null)
                    return false;
            }
        }
        return true;
    }

    private bool IsSquareAttacked(Vector2Int targetSquare, ChessColor attackerColor, ChessBoard board)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece? piece = board.Board[x, y];
                if (piece != null)
                {
                    if (piece.Color == attackerColor)
                    {
                        if (piece.CanAttack(piece.Position, targetSquare))
                        {
                            if (piece.NeedsPathCheck)
                            {
                                if (IsPathClear(piece.Position, targetSquare, board))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}