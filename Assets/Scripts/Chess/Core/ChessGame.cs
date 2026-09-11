using UnityEngine;

public class ChessGame : MonoBehaviour
{
    private ChessBoard board;
    private BoardSetup boardSetup;
    private MoveValidator validator;
    private MoveExecutor moveExecutor;
    private ChessColor currentTurn = ChessColor.White;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = new ChessBoard();
        boardSetup = new BoardSetup();
        validator = new MoveValidator();
        moveExecutor = new MoveExecutor();

        boardSetup.Setup(board);
        TestGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakeMove(Move move)
    {
        if (move.Piece.Color != currentTurn)
            return;

        if (!validator.IsValidMove(move, board))
            return;

        moveExecutor.ExecuteMove(move, board);

        currentTurn = currentTurn == ChessColor.White ? ChessColor.Black : ChessColor.White;
    }

    private void TestGame()
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

        MakeMove(move);

        Debug.Log(currentTurn);
    }
}
