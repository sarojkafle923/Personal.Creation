using System;
using System.Collections.Generic;
using System.Linq;
using DocuSign.Client.Library.Interfaces;
using DocuSign.Client.Library.Models.Login;
using DocuSign.Client.Library.Models.Request;
using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;

namespace DocuSign.Client.Library.Services;

public class UserLoginService : IUserLoginService
{
    public Login UpdateLogin(Login loginDetail, LoginRequest request)
    {
        if (loginDetail is not null && loginDetail.User.ExpireIn.Value.Minute - DateTime.Now.Minute > 5)
            return loginDetail;
        var updatedLoginDetail = Login(request);
        return updatedLoginDetail;
    }

    public Login Login(LoginRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var authToken = GetOAuthToken(request.ClientId, request.userId, request.AuthServer, request.PrivateKeyBase64);
        if (authToken is null) return null;

        var account = GetAccount(authToken, request.AuthServer);

        var user = new User
        {
            Name = account.AccountName,
            AccessToken = authToken.access_token,
            ExpireIn = DateTime.Now.AddSeconds(3600),
            AccountId = account.AccountId
        };

        var session = new Session
        {
            AccountId = account.AccountId,
            AccountName = account.AccountName,
            BasePath = account.BaseUri
        };

        var login = new Login();
        login.Session = session;
        login.User = user;

        return login;
    }

    public bool IsLoginRequire(User user)
    {
        return user is null || user.ExpireIn.Value.Minute - DateTime.Now.Minute <= 5;
    }

    public Login Logout()
    {
        return new Login
        {
            Session = new Session
            {
                AccountId = null,
                AccountName = null,
                BasePath = null
            },
            User = new User
            {
                AccessToken = null,
                AccountId = null,
                ExpireIn = DateTime.Now,
                Name = null
            }
        };
    }

    private OAuth.OAuthToken GetOAuthToken(string clientId, string userId, string authServer,
        string privateKeyBase64)
    {
        var apiClient = new DocuSignClient();
        var scopes = new List<string>
        {
            "signature",
            "impersonation"
        };

        var privateKeyFileByteArray = Convert.FromBase64String(privateKeyBase64);

        try
        {
            var authToken = apiClient.RequestJWTUserToken(clientId, userId, authServer, privateKeyFileByteArray, 1,
                scopes);
            return authToken;
        }
        catch (ApiException exception)
        {
            if (exception.Message.Contains("consent_required")) return null;

            throw new Exception(exception.Message);
        }
    }

    private OAuth.UserInfo.Account GetAccount(OAuth.OAuthToken authToken, string authServer)
    {
        var apiClient = new DocuSignClient();
        apiClient.SetOAuthBasePath(authServer);
        var userInfo = apiClient.GetUserInfo(authToken.access_token);
        var account = userInfo.Accounts.FirstOrDefault();
        if (account is null) throw new Exception("The user doesn't have access to account details");

        return account;
    }
}