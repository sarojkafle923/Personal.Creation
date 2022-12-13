using System.Threading.Tasks;
using DocuSign.Client.Library.Interfaces;
using DocuSign.Client.Library.Models.Login;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;

namespace DocuSign.Client.Library.Services;

public class EnvelopeFormDataService : IEnvelopeFormDataService
{
    public async Task<EnvelopeFormData> GetFormData(Login loginDetail, string envelopeId)
    {
        var apiClient = new DocuSignClient(loginDetail.Session.BasePath + "/restapi");
        apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + loginDetail.User.AccessToken);

        var envelopeApi = new EnvelopesApi(apiClient);
        return await envelopeApi.GetFormDataAsync(loginDetail.User.AccountId, envelopeId);
    }
}