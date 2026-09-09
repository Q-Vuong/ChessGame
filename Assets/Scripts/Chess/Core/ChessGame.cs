using UnityEngine;

public class ChessGame : MonoBehaviour
{
    private ChessBoard board;
    private BoardSetup boardSetup;
    private MoveValidator validator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = new ChessBoard();
        boardSetup = new BoardSetup();
        validator = new MoveValidator();

        boardSetup.Setup(board);
        TestGame();
    }

    // Update is called once per frame
    void Update()
    {

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

        Pawn pawn = new Pawn(ChessColor.White, new Vector2Int(3, 3));
        Pawn pawnBlack = new Pawn(ChessColor.Black, new Vector2Int(4, 4));

        board.AddPiece(pawnBlack, pawnBlack.Position);

        Move move = new Move(
            pawn,
            new Vector2Int(3, 3),
            new Vector2Int(3, 4)
        );

        bool isValid = validator.IsValidMove(move, board);

        Debug.Log(isValid);
    }
}
