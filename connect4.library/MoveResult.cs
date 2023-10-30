namespace connect4.library;
public class MoveResult
{
    public GameBoard BoardState { get; set; } = new GameBoard();
    public bool IsValid { get; set; } = true;
    public string ErrorMessage { get; set; } = string.Empty;

}