using System;
using System.Threading.Tasks;
using DocuSign.Client.Library.Interfaces;
using DocuSign.Client.Library.Models.Login;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;

namespace DocuSign.Client.Library.Services;

public class EnvelopeStatusService : IEnvelopeStatusService
{
    public async Task<string> GetEnvelopeStatusService(Login loginDetail, string envelopeId)
    {
        if (envelopeId is null) throw new ArgumentNullException(nameof(envelopeId));

        var apiClient = new DocuSignClient(loginDetail.Session.BasePath + "/restapi");
        apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + loginDetail.User.AccessToken);

        var envelopesApi = new EnvelopesApi(apiClient);
        var envelopeInformation = await envelopesApi.GetEnvelopeAsync(loginDetail.User.AccountId, envelopeId);
        return envelopeInformation.Status;
    }
}