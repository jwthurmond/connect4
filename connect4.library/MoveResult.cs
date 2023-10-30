namespace connect4.library;
public class MoveResult
{
    public GameBoard BoardState { get; set; }
    public bool IsValid { get; set; } = true;
    public string ErrorMessage { get; set; }

}