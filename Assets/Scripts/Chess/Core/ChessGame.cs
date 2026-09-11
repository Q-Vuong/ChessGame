using UnityEngine;

public class ChessGame : MonoBehaviour
{
    private ChessBoard board;
    private BoardSetup boardSetup;
    private MoveValidator validator;
    private MoveExecutor moveExecutor;
    private GameStateEvaluator stateEvaluator;

    private ChessColor currentTurn = ChessColor.White;

    private bool gameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = new ChessBoard();
        boardSetup = new BoardSetup();
        validator = new MoveValidator();
        moveExecutor = new MoveExecutor();
        stateEvaluator = new GameStateEvaluator(validator);

        boardSetup.Setup(board);

        TestGame testGame = new TestGame();
        testGame.RunAllTests();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakeMove(Move move)
    {
        if (gameOver)
        {
            Debug.Log("Game Finished cannot move");
            return;
        }

        if (move.Piece.Color != currentTurn)
            return;

        if (!validator.IsValidMove(move, board))
            return;

        moveExecutor.ExecuteMove(move, board);

        currentTurn = currentTurn == ChessColor.White ? ChessColor.Black : ChessColor.White;

        GameState state = stateEvaluator.Evaluate(currentTurn, board);

        switch (state)
        {
            case GameState.Normal:
                Debug.Log("NORMAL => Continue");
                break;
            case GameState.Check:
                Debug.Log("CHECK => Continue");
                break;
            case GameState.Checkmate:
                gameOver = true;
                Debug.Log("CHECKMATE => Game Finish");
                break;
            case GameState.Stalemate:
                gameOver = true;
                Debug.Log("Stalemate => Game Finish");
                break;
        }
    }
}
