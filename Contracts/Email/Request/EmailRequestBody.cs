namespace rmsbe.Contracts.Email.Request;

public class EmailRequestBody
{
    public IList<string> To { get; set; }
    
    public string Text { get; set; }
}