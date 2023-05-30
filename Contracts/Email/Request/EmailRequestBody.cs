namespace rmsbe.Contracts.Email.Request;

public class EmailRequestBody
{
    public IList<string> To { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}