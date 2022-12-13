using System.Threading.Tasks;
using DocuSign.Client.Library.Interfaces;
using DocuSign.Client.Library.Models.Login;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;

namespace DocuSign.Client.Library.Services;

public class SendEnvelopeViaEmailService : ISendEnvelopeViaEmailService
{
    public async Task<EnvelopeSummary> SendEnvelopeViaEmail(Login loginDetail, EnvelopeDefinition envelopeDefinition)
    {
        var apiClient = new DocuSignClient(loginDetail.Session.BasePath + "/restapi");
        apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + loginDetail.User.AccessToken);

        var envelopeApi = new EnvelopesApi(apiClient);
        var response = await envelopeApi.CreateEnvelopeAsync(loginDetail.User.AccountId, envelopeDefinition);
        return response;
    }
}