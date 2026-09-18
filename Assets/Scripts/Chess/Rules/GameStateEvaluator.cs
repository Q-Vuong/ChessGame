using System.Collections.Generic;
using UnityEngine;

public class GameStateEvaluator
{

    private MoveValidator validator;

    public GameStateEvaluator(MoveValidator validator)
    {
        this.validator = validator;
    }

    private bool HasLegalMove(ChessColor color, ChessBoard board, Move lastMove)
    {
        List<ChessPiece> pieces = board.GetPieces(color);

        foreach (ChessPiece piece in pieces)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Vector2Int targetSquare = new Vector2Int(x, y);

                    if (targetSquare == piece.Position)
                        continue;

                    Move move = new Move(piece, piece.Position, targetSquare);

                    if (validator.IsValidMove(move, lastMove, board))
                        return true;
                }
            }
        }
        return false;
    }

    public GameState Evaluate(ChessColor color, ChessBoard board, Move lastMove)
    {
        King king = board.FindKing(color);

        ChessColor attackerColor = color == ChessColor.White ? ChessColor.Black : ChessColor.White;

        if (validator.IsSquareAttacked(king.Position, attackerColor, board))
        {
            if(HasLegalMove(color, board, lastMove))
            {
                return GameState.Check;
            }
            else
            {
                return GameState.Checkmate;
            }
        }
        else
        {
            if(HasLegalMove(color, board, lastMove))
            {
                return GameState.Normal;
            }
            else
            {
                return GameState.Stalemate;
            }
        }
    }
}