namespace SolExB2BApiDemo.Model;

public class TokenApi
{
    public string AccessToken { get; set; }

    public int ExpiresIn { get; set; }

    public string TokenType { get; set; }
}