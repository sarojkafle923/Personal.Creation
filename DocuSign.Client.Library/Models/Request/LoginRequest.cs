namespace DocuSign.Client.Library.Models.Request;

public class LoginRequest
{
    public string ClientId { get; set; }

    public string userId { get; set; }

    public string AuthServer { get; set; }

    public string PrivateKeyBase64 { get; set; }
}