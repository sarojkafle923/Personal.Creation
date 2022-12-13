using DocuSign.Client.Library.Models.Login;
using DocuSign.Client.Library.Models.Request;

namespace DocuSign.Client.Library.Interfaces;

public interface IUserLoginService
{
    public Login Login(LoginRequest request);

    public bool IsLoginRequire(User user);
}