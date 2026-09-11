using UnityEngine;

public class TestGame
{
    private ChessBoard board;
    private MoveValidator validator;
    private GameStateEvaluator evaluator;

    public TestGame()
    {
        board = new ChessBoard();
        validator = new MoveValidator();
        evaluator = new GameStateEvaluator(validator);
    }

    public void RunAllTests()
    {
        //TestMove();
        TestNormal();
        TestCheck();
        TestCheckmate();
        TestStalemate();
    }

    private void TestMove()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = board.GetPiece(new Vector2Int(x, y));

                if (piece != null)
                {
                    Debug.Log($"{piece.GetType().Name} - {piece.Color} - {piece.Position}");
                }
            }
        }

        // ------------------------------

        Pawn pawn = new Pawn(ChessColor.White, new Vector2Int(3, 1));
        Knight knight = new Knight(ChessColor.White, new Vector2Int(1, 0));
        Pawn pawnBlack = new Pawn(ChessColor.Black, new Vector2Int(1, 1));
        Pawn pawnBlack1 = new Pawn(ChessColor.Black, new Vector2Int(1, 2));

        board.AddPiece(pawnBlack, pawnBlack.Position);
        board.AddPiece(pawnBlack1, pawnBlack1.Position);

        //Move move = new Move(pawn, new Vector2Int(3, 1), new Vector2Int(3, 3));
        Move move = new Move(knight, new Vector2Int(1, 0), new Vector2Int(2, 2));
    }

    private void TestNormal()
    {
        board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(4, 7)
        );

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board
        );

        Debug.Log($"TestNormal: {state}");
    }

    private void TestCheck()
    {
        board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(0, 7)
        );

        Rook blackRook = new Rook(
            ChessColor.Black,
            new Vector2Int(4, 7)
        );

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);
        board.AddPiece(blackRook, blackRook.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board
        );

        Debug.Log($"TestCheck: {state}");
    }

    private void TestCheckmate()
    {
        board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(0, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(2, 2)
        );

        Queen blackQueen = new Queen(
            ChessColor.Black,
            new Vector2Int(1, 1)
        );

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);
        board.AddPiece(blackQueen, blackQueen.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board
        );

        Debug.Log($"TestCheckmate: {state}");
    }

    private void TestStalemate()
    {
        board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(0, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(2, 2)
        );

        Queen blackQueen = new Queen(
            ChessColor.Black,
            new Vector2Int(2, 1)
        );

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);
        board.AddPiece(blackQueen, blackQueen.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board
        );

        Debug.Log($"TestStalemate: {state}");
    }
}