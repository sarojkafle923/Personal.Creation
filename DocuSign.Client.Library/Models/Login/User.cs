using System;

namespace DocuSign.Client.Library.Models.Login;

public class User
{
    public string Name { get; set; }

    public string AccessToken { get; set; }

    public DateTime? ExpireIn { get; set; }

    public string AccountId { get; set; }
}