public class MoveExecutorResult
{
    public ChessPiece? CapturedPiece { get; }
    public ChessPiece? NewPiece { get; }

    public MoveExecutorResult(ChessPiece? capturedPiece, ChessPiece? newPiece)
    {
        CapturedPiece = capturedPiece;
        NewPiece = newPiece;
    }
}