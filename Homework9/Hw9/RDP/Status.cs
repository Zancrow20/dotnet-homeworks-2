namespace Hw9.ReversePolishNotation;

public class Status
{
    public string ErrorMessage { get; set; } = string.Empty;
    
    public bool IsGoodExpression { get; set; } = true;

    public Status(string errorMessage)
    {
        IsGoodExpression = false;
        ErrorMessage = errorMessage;
    }
    
    public Status(){ }
}